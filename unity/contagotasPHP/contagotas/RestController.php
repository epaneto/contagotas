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
		
	case "create":
		// to handle REST Url /group/create/<group name>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->createGroup($_GET["group_name"]);
		break;
		
	case "join":
		// to handle REST Url /group/join/<group name>/<user_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->joinGroup($_GET["group_name"],$_GET["user"]);
		break;
		
	case "hasGroup":
		// to handle REST Url /group/join/<group name>/<user_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->hasGroup($_GET["user"]);
		break;
		
	case "leave":
		// to handle REST Url /group/join/<group name>/<user_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->leaveGroup($_GET["user"]);
		break;

	case "" :
		//404 - not found;
		break;
}
?>
