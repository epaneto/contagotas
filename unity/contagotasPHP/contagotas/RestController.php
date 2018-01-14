<?php
require_once("GroupRestHandler.php");
require_once("UserRestHandler.php");
		
$view = "";
if(isset($_GET["view"]))
	$view = $_GET["view"];

/*if($_SERVER['HTTP_USER_AGENT'] != "app-contagotas")
{
	die("ERROR!");
}*/

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
	
	case "single_by_name":
		// to handle REST Url /group/list_by_name/<group_name>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->getGroupByName($_GET["group_name"]);
		break;
		
	case "create":
		// to handle REST Url /group/create/
		$groupRestHandler = new GroupRestHandler();
		
		$data = $_POST;
		$json = $data["data"];
		$json_decoded = json_decode($json);

		$groupRestHandler->createGroup(utf8_decode ($json_decoded->name), utf8_decode($json_decoded->password));
		break;
		
	case "join":
		// to handle REST Url /group/join/<group name>/<user_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->joinGroup($_GET["group"],$_GET["user"]);
		break;
		
	case "join_with_password":
		// to handle REST Url /group/join/
		$groupRestHandler = new GroupRestHandler();
		
		$data = $_POST;
		$json = $data["data"];
		$json_decoded = json_decode($json);
		
		$groupRestHandler->joinGroupWithPassword(utf8_decode ($json_decoded->groupid), utf8_decode($json_decoded->userid), utf8_decode($json_decoded->password));
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
		$groupRestHandler->InsertGroupScore($_GET["group_id"],$_GET["score"],$_GET["user_id"]);
		break;
	
	case "score_get":
		// to handle REST Url /group/score/get/<group_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->getGroupScore($_GET["group_id"]);
		break;
	case "score_get_detailed":
		// to handle REST Url /group/score/get_detailed/<group_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->getGroupScoreDetailed($_GET["group_id"]);
		break;
	case "score_top":
		// to handle REST Url /group/score/top/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->getTopGroupScore();
		break;
		
	case "create_user":
		// to handle REST Url /user/create/<user name>/<email>/<city>/<state>/<facebook_id>/
		$userRestHandler = new UserRestHandler();
		
		$data = $_POST;
		$json = $data["data"];
		$json_decoded = json_decode($json);
		 
		$userRestHandler->createUser(utf8_decode ($json_decoded->name),utf8_decode ($json_decoded->email),utf8_decode ($json_decoded->city), utf8_decode ($json_decoded->state), utf8_decode ($json_decoded->facebookId));
		break;
	case "user_score_insert":
		// to handle REST Url /user/score/<user_id>/<score>/
		$userRestHandler = new UserRestHandler();
		$userRestHandler->insertScore($_GET["user_id"],$_GET["score"]);
		break;
	case "user_get_score":
		// to handle REST Url /user/score/<user_id>/
		$userRestHandler = new UserRestHandler();
		$userRestHandler->getUserScore($_GET["user_id"]);
		break;
	case "user_score_ranking":
		// to handle REST Url /user/score/top10/
		$userRestHandler = new UserRestHandler();
		$userRestHandler->getUserRanking();
		break;
	case "invite_create":
		// to handle REST Url /group/invite/create/<user inviter id>/<user invite name>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->createInvite($_GET["inviter_id"],$_GET["facebook_id"]);
		break;
	case "invite_check":
		// to handle REST Url /group/invite/check/<user_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->checkInvite($_GET["user"]);
		break;
	case "invite_accept":
		// to handle REST Url /group/invite/accept/<invite_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->acceptInvite($_GET["invite"]);
		break;
	case "invite_deny":
		// to handle REST Url /group/invite/check/<invite_id>/
		$groupRestHandler = new GroupRestHandler();
		$groupRestHandler->denyInvite($_GET["invite"]);
		break;
	
	case "" :
		echo "view not found! Error!";
		break;
}
?>
