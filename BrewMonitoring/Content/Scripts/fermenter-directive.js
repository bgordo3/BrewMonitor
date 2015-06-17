BrewMonitor.directive('fermenters', function($compile, $http) 
{
    var Link = function ($scope, $element) 
	{

		//--------------------
		// SetID
		var SetID = function()
		{
			$http.post('Fermenter/SetID', {TransientID : $scope.Fermenter, Id : $scope.NewID}).then(
			function(res)
			{    
				UpdateData();              
			});
		}

		//--------------------
		// Update Data
		var UpdateData = function ()
		{
			$http.get('Fermenter/').then(
			function(res)
			{
			  	$scope.Fermenters = res.data;              
			  	$scope.NewID = "Renommer le fermenteur";             
			}); 
		}

		UpdateData();
		$scope.SetID = SetID;
		$scope.UpdateData = UpdateData

		//--------------------
    	//Set parent panel
    	$scope.$parent.PanelClasses = "col-md-4"; 
		$scope.$parent.PanelTitle = "Fermenteurs"; 
   }

	return {
	    restrict: 'E',
	    templateUrl: 'Content/html/templates/Hardware/Fermenter.html',
	    link: Link
  	};
});

