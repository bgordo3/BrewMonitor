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

	var TicksToDate = function (Ticks)
	{
		//ticks are in nanotime; convert to microtime
		var ticksToMicrotime = Ticks / 10000;
		 
		//ticks are recorded from 1/1/1; get microtime difference from 1/1/1/ to 1/1/1970
		var epochMicrotimeDiff = 2208988800000;
		 
		//new date is ticks, converted to microtime, minus difference from epoch microtime
		return new Date(ticksToMicrotime - epochMicrotimeDiff);
	};

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
		var UpdateData = function ()
		{
			$http.get('Batch/Get/' + ObjectId($scope.Batch._id)).then(
			function(res)
			{
			  	$scope.Batch = res.data;                   
			});

			$http.get('Recipe/Get/' + ObjectId($scope.Batch.BeerRecipe)).then(
			function(res)
			{
			  	$scope.Recipe = res.data;              
			}); 

			$http.get('Fermenter/').then(
			function(res)
			{
			  	$scope.Fermenters = res.data;              
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
    	var UpdateInterval = $interval($scope.UpdateData, 5000);
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
     				chartData.addRow([TicksToDate(value.Item1), value.Item2, undefined]);
			});

  			chartData.sort([{column: 0}]);
     		chart.draw(chartData, chartOptions);	
     	}, true);

     	$scope.$watch('Recipe.FermentationTemperatureCurve.Values', function(newValues, OldValues, Target) 
     	{
	     	angular.forEach(newValues, function(value, key) 
	     	{
			  if (typeof OldValues === 'undefined' || $.inArray(value, OldValues) !== 1)
     				chartData.addRow([TicksToDate(value.Item1), undefined,  value.Item2]);
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

