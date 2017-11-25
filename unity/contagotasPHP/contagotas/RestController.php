<?php
require_once("GroupRestHandler.php");
		
$view = "";
if(isset($_GET["view"]))
	$view = $_GET["view"];
/*
controls the RESTful services
URL mapping
*/


switch($view){

	case "all":
		// to handle REST Url /group/list/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->getAllGroups();
		break;
		
	case "single":
		// to handle REST Url /group/show/<id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->getGroup($_GET["id"]);
		break;
		
	case "single_by_user_id":
		// to handle REST Url /group/show/<id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->getGroupByUserId($_GET["id"]);
		break;
		
	case "create":
		// to handle REST Url /group/create/<group name>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->createGroup($_GET["group_name"]);
		break;
		
	case "join":
		// to handle REST Url /group/join/<group name>/<user_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->joinGroup($_GET["group"],$_GET["user"]);
		break;
		
	case "hasGroup":
		// to handle REST Url /group/hasGroup/<user_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->hasGroup($_GET["user"]);
		break;
		
	case "leave":
		// to handle REST Url /group/leave/<user_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->leaveGroup($_GET["user"]);
		break;

	case "score_insert":
		// to handle REST Url /group/join/score/<score>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->InsertGroupScore($_GET["group_id"],$_GET["score"]);
		break;
	
	case "score_get":
		// to handle REST Url /group/score/get/<group_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->getGroupScore($_GET["group_id"]);
		break;
		
	case "score_top":
		// to handle REST Url /group/score/top/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->getTopGroupScore();
		break;
	
	case "" :
		echo "view not found! Error!";
		break;
}
?>
