"use strict";

google.load('visualization', '1.0',  {'packages' :['corechart']});

/**
* Main AngularJS Web Application
*/
var BrewMonitor = angular.module('BrewMonitor', ['ngRoute']);

//Controller
//------------------------------ 
BrewMonitor.controller('NavMenuCtrl', function ( $scope, $location, $http, $compile ) 
{
	var UpdateNavMenu = function()
	{
		$http.get('Batch/CurrentBatches').then(
		function(res)
		{
		  	$scope.CurrentBatches = res.data;                               
		});
	}

	UpdateNavMenu();

	$scope.ShowRecipeNav = function()
	{
		$http.get('Recipe/').then(
		function(res)
		{
		  	//Create the batch panel
	 		var recipenavpanel = $compile("<panel><recipenav></recipenav></panel>");
	 		//setup new scope
	 		var recipescope = $scope.$new(true);
	 		recipescope.Recipes = res.data;
	 		recipescope.BeerTypes = [];
	 		for (var Recipe in res.data)
	 		{
	 			if (recipescope.BeerTypes.indexOf(Recipe.BeerType) == -1)
	 			{
	 				recipescope.BeerTypes.push(Recipe.BeerType);
	 			}
	 		}
	 		//Add to page
	   		$('#main-container').prepend(recipenavpanel(recipescope));
   		                           
		});

	};

	$scope.ShowFermenters = function()
	{
		//Create the batch panel
 		var recipenavpanel = $compile("<panel><fermenters></recipenav></fermenters>");
 		//setup new scope
 		var recipescope = $scope.$new(true);
 		//Add to page
   		$('#main-container').prepend(recipenavpanel(recipescope));

	};

	$scope.CreateBatch = function()
	{
		$http.post('Batch/Create/').then(
		function(res)
		{     
			UpdateNavMenu();                     
		});
	}
});

BrewMonitor.controller('PanelCtrl', function ( $scope, $location, $http ) 
{
	$scope.PanelTitle = "Panel";                            
	$scope.PanelClasses = "col-md-5";     
});

BrewMonitor.controller('RecipeCtrl', function ( $scope, $location, $http ) 
{
	$http.get('Recipe/' + $scope.Recipe.IdStr).then(
	function(res)
	{
	  	$scope.recipe = res.data;                               
	});
});

BrewMonitor.controller('FermenterCtrl', function ( $scope, $location, $http ) 
{
	$http.get('Batch/CurrentBatches').then(
	function(res)
	{
	  	$scope.CurrentBatches = res.data;                               
	});
});

BrewMonitor.controller('RecipeNavCtrl', function ( $scope, $location, $http ) 
{
	$scope.$parent.$parent.PanelTitle = "Recettes";
});

//Directives
//------------------------------ 
BrewMonitor.directive('panel', function() 
{
	return {
	    restrict: 'E',
	    transclude: true,
	    link: function ($scope, $element) 
	    {
            $scope.close = function () 
            {
            	$element.remove();
            };
        },
	    templateUrl: 'Content/html/templates/Global/Panel.html'
  	};
});

//Batch
//------------------------------ 
BrewMonitor.directive('batchname', function($compile) 
{
	return {
	    restrict: 'E',
	    link: function ($scope, $element) 
	    {
	    	//Add a batch panel on click
	    	$element.bind('click', function() {
        		//Create the batch panel
	     		var batchpanel = $compile("<panel><batch></batch></panel>");
	     		//setup new scope
	     		var batchscope = $scope.$new(true);
	     		batchscope.Batch = $scope.Batch;
	     		//Add to page
           		$('#main-container').prepend(batchpanel(batchscope));
      		});
      	},
	    templateUrl: 'Content/html/templates/Batch/BatchName.html'
  	};
});



//Recipe
//------------------------------ 
BrewMonitor.directive('recipename', function($compile) 
{
	return {
	    restrict: 'E',
	    link: function ($scope, $element) 
	    {
	    	//Add a batch panel on click
	    	$element.bind('click', function() {
        		//Create the batch panel
	     		var recipepanel = $compile("<panel><recipe></recipe></panel>");
	     		//setup new scope
	     		var recipescope = $scope.$new(true);
	     		recipescope.Recipe = $scope.Recipe;
	     		//Add to page
           		$('#main-container').prepend(recipepanel(recipescope));
      		});
      	},
	    templateUrl: 'Content/html/templates/Recipe/RecipeName.html'
  	};
});

BrewMonitor.directive('recipe', function($compile) 
{
	return {
	    restrict: 'E',
	    templateUrl: 'Content/html/templates/Recipe/Recipe.html'
  	};
});

BrewMonitor.directive('recipenav', function($compile) 
{
	return {
	    restrict: 'E',
	    templateUrl: 'Content/html/templates/Recipe/RecipeNav.html'
  	};
});

//Filters
//------------------------------ 
BrewMonitor.filter('netdate', function ($filter) {
    return function (input, format) {
        return $filter('date')(parseInt(input.substr(6)), format);
    };
});