<?php
require_once("SimpleRest.php");
require_once("Group.php");
require_once("Response.php");
		
class GroupRestHandler extends SimpleRest {

	function getAllGroups() {	

		$group = new Group();
		$rawData = $group->getAllGroup();

		$response = new Response();
		$response->send($rawData);
	}
		
	public function getGroup($id) {

		$group = new Group();
		$rawData = $group->getGroup($id);

		$response = new Response();
		$response->send($rawData);
	}
	
	public function getGroupByName($groupName) {

		$group = new Group();
		$rawData = $group->getGroupByName($groupName);

		$response = new Response();
		$response->send($rawData);
	}
	
	public function getGroupByUserId($id) {

		$group = new Group();
		$rawData = $group->getGroupByUserId($id);

		$response = new Response();
		$response->send($rawData);
	}
	
	
	public function createGroup($group_name) {

		$group = new Group();
		$rawData = $group->createGroup($group_name);

		$response = new Response();
		$response->send($rawData);
	}
	
	
	public function leaveGroup($user) {

		$group = new Group();
		$rawData = $group->leaveGroup($user);

		$response = new Response();	
		$response->send($rawData);
	}
	
	public function hasGroup($group_name) {

		$group = new Group();
		$rawData = $group->hasGroup($group_name);
		
		$statusCode = 200;
		
		if($rawData)
			$rawData = "true";
		else
			$rawData = "false";
		
		$response = new Response();
		$response->send($rawData);
	}
	
	public function joinGroup($group_id,$user_id) {

		$group = new Group();
		$rawData = $group->joinGroup($group_id,$user_id);
		
		$response = new Response();
		$response->send($rawData);
	
	}
	
	public function InsertGroupScore($group_id,$score) {

		$group = new Group();
		$rawData = $group->insertGroupScore($group_id,$score);

		$response = new Response();
		$response->send($rawData);
	}
	
	public function getGroupScore($group_id) {

		$group = new Group();
		$rawData = $group->getGroupScore($group_id);

		$response = new Response();
		$response->send($rawData);
	}
	
	public function getTopGroupScore() {

		$group = new Group();
		$rawData = $group->getTopGroupScore();

		$response = new Response();
		$response->send($rawData);
	}
	
	public function createInvite($user_id, $facebook_id) {

		$group = new Group();
		$rawData = $group->createInvite($user_id, $facebook_id);

		$response = new Response();
		$response->send($rawData);
	}
	
	public function checkInvite($user_id) {

		$group = new Group();
		$rawData = $group->checkInvite($user_id);

		$response = new Response();
		$response->send($rawData);
	}
	
	public function acceptInvite($invite_id) {

		$group = new Group();
		$rawData = $group->acceptInvite($invite_id);

		$response = new Response();
		$response->send($rawData);
	}
	
	public function denyInvite($invite_id) {

		$group = new Group();
		$rawData = $group->denyInvite($invite_id);

		$response = new Response();
		$response->send($rawData);
	}
	
}
?>