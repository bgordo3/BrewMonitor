BrewMonitor.directive('batch', function($compile, $interval, $http) 
{
	var chart;
	var chartData;
	var chartOptions;

	var ConfigureChart = function(element)
	{
		//Configure the chart.
		chart = new google.visualization.AreaChart(element);
		chartData = new google.visualization.DataTable();
		chartData.addColumn('date', 'Temps');
		chartData.addColumn('number', 'Actuel');
		chartData.addColumn('number', 'Visée');
	    chartOptions = 
	    {
	      title: "Fermentation",
	      titleTextStyle:{color: '#FFF', fontSize:20, fontName:"Lato:300,400,700", bold:false},
	      backgroundColor : "transparent",
	      vAxis: 
	      {
	        title: "Température",
	        gridlines: {"count": 10},
	      	minValue: 15,
	      	textStyle:{color: '#FFF',fontName:"Lato", fontSize:12,italic:false},
	      	titleTextStyle:{color: '#FFF',fontName:"Lato", fontSize:16,italic:false},
	      	baselineColor:"#fff"
	      },
	      lineWidth : 2,
	      legend: 'none',
	     /* legend:
	      {
	      	textStyle:{color: '#FFF',fontName:"Lato", fontSize:12,italic:false}
	      },*/
	      colors: ['#FF5555', '#5555FF'],
	      interpolateNulls: "true",
	      hAxis: 
	      {
	      	title: "",
	      	textStyle:{color: '#FFF',fontName:"Lato", fontSize:12,italic:false},
	      	titleTextStyle:{color: '#FFF',fontName:"Lato", fontSize:16,italic:false},
	      	baselineColor:"#fff"
	      },
        animation: {
	        duration: 1000,
	        easing: 'out'
	      }
		};
	}



	var Link = function ($scope, $element) 
	{

		//--------------------
		// SAVE
		var Save = function()
		{
			$scope.Batch._id = ObjectId($scope.Batch._id).toString();
			$scope.Batch.BeerRecipe = ObjectId($scope.Batch.BeerRecipe).toString();
			$http.post('Batch/Update/', $scope.Batch).then(
			function(res)
			{                  
			});
		}

		//--------------------
		// DELETE
		var Delete = function()
		{
			$http.post('Batch/Delete/' + ObjectId($scope.Batch._id), {}).then(
			function(res)
			{
			  	$scope.close();                     
			});
		}

		//--------------------
		// Update Data
		var Update = 0;
		var UpdateData = function ()
		{
			$http.get('Batch/Get/' + ObjectId($scope.Batch._id)).then(
			function(res)
			{
			  	$scope.Batch = res.data;
			  	if (Update == 0)
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,16,48), 15.2]); 
			  	if (Update == 1)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,17,32), 15.1]); 
			  	if (Update == 2)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,18,16), 15.1]);
			  	if (Update == 3)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,25,06), 14.1]); 
			  	if (Update == 4)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,18,46), 15.0]); 
			  	if (Update == 5)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,19,32), 15.0]); 
			  	if (Update == 6)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,23,37), 14.5]); 
			  	if (Update == 7)
			  	  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,20,15), 14.8]); 
			  	if (Update == 8)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,21,01), 14.85]); 
			  	if (Update == 9)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,21,37), 14.8]); 
			  	if (Update == 10)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,24,28), 14.2]); 
			  	if (Update == 11)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,22,15), 14.9]); 
			  	if (Update == 12)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,22,53), 14.6]); 
			  	if (Update == 13)
			  	
			  	$scope.Batch.FermentationMesures.Curve.Values.push([new Date(2015,6,2, 15,25,54), 14.15]); 

			  	                                Update = Update + 1;                                
			});

			$http.get('Recipe/Get/' + ObjectId($scope.Batch.BeerRecipe)).then(
			function(res)
			{
			  	$scope.Recipe = res.data;              
			}); 
		}
		$scope.Save = Save;
		$scope.Delete = Delete;
		$scope.UpdateData = UpdateData

		//--------------------
    	//Set parent panel
    	$scope.$parent.PanelClasses = "col-md-7"; 
    	$scope.$watch('Batch.IsDone', function(IsDone)
    	{
    		$scope.$parent.PanelTitle = "Unité de production" + ($scope.Batch.IsDone? "" : " - en production");                          
    	});
    	  
    	//-------------------
    	//Setup timely updates
    	var UpdateInterval = $interval($scope.UpdateData, 2000);
		// listen on DOM destroy (removal) event, and cancel the next UI update
		$element.on('$destroy', function() {
			$interval.cancel(UpdateInterval);
		});

		//Configure chart.
    	ConfigureChart($element.find(".batchchart")[0]);

     	$scope.$watch('Batch.FermentationMesures.Curve.Values', function(newValues, OldValues) 
     	{
	     	angular.forEach(newValues, function(value, key) 
	     	{
			  if (typeof OldValues === 'undefined' || $.inArray(value, OldValues) !== 1)
     				chartData.addRow([new Date(value[0]), value[1], undefined]);
			});

  			chartData.sort([{column: 0}]);
     		chart.draw(chartData, chartOptions);	
     	}, true);

     	$scope.$watch('Recipe.FermentationTemperatureCurve.Values', function(newValues, OldValues, Target) 
     	{
	     	angular.forEach(newValues, function(value, key) 
	     	{
			  if (typeof OldValues === 'undefined' || $.inArray(value, OldValues) !== 1)
     				chartData.addRow([new Date(value[0]), undefined,  value[1]]);
			});

  			chartData.sort([{column: 0}]);
     		chart.draw(chartData, chartOptions);	
     	}, true);

     	chart.draw(chartData, chartOptions);
   }

	return {
	    restrict: 'E',
	    templateUrl: 'Content/html/templates/Batch/Batch.html',
	    link: Link
  	};
});

