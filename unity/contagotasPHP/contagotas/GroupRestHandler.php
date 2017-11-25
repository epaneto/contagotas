<?php
require_once("SimpleRest.php");
require_once("Group.php");
		
class GroupRestHandler extends SimpleRest {

	function getAllGroups() {	

		$group = new Group();
		$rawData = $group->getAllGroup();

		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No groups found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
				
		if(strpos($requestContentType,'application/json') !== false){
			$response = $this->encodeJson($rawData);
			echo $response;
		} else if(strpos($requestContentType,'text/html') !== false){
			$response = $this->encodeHtml($rawData);
			echo $response;
		} else if(strpos($requestContentType,'application/xml') !== false){
			$response = $this->encodeXml($rawData);
			echo $response;
		}
	}
	
	public function encodeHtml($responseData) {
		$htmlResponse = $responseData;
		/*$htmlResponse = "<table border='1'>";
		foreach($responseData as $key=>$value) {
    			$htmlResponse .= "<tr><td>". $key. "</td><td>". $value. "</td></tr>";
		}
		$htmlResponse .= "</table>";*/
		return $htmlResponse;		
	}
	
	public function encodeJson($responseData) {
		$jsonResponse = json_encode($responseData);
		return $jsonResponse;		
	}
	
	public function encodeXml($responseData) {
		// creating object of SimpleXMLElement
		$xml = new SimpleXMLElement('<?xml version="1.0"?><mobile></mobile>');
		foreach($responseData as $key=>$value) {
			$xml->addChild($key, $value);
		}
		return $xml->asXML();
	}
	
	public function getGroup($id) {

		$group = new Group();
		$rawData = $group->getGroup($id);

		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No groups found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
			
		echo $rawData;
	}
	
	public function getGroupByUserId($id) {

		$group = new Group();
		$rawData = $group->getGroupByUserId($id);

		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No groups found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
			
		echo $rawData;
	}
	
	
	public function createGroup($group_name) {

		$group = new Group();
		$rawData = $group->createGroup($group_name);

		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No group name found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
		
		echo $rawData;		
		/*if(strpos($requestContentType,'application/json') !== false){
			echo "result === ".$rawData;
			$response = $this->encodeJson($rawData);
			echo $response;
		} else if(strpos($requestContentType,'text/html') !== false){
			$response = $this->encodeHtml();
			echo $response;
		} else if(strpos($requestContentType,'application/xml') !== false){
			$response = $this->encodeXml($rawData);
			echo $response;
		}*/
	}
	
	
	public function leaveGroup($user) {

		$group = new Group();
		$rawData = $group->leaveGroup($user);

		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No group name found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
		
		echo $rawData;		
	}
	
	public function hasGroup($group_name) {

		$group = new Group();
		$rawData = $group->hasGroup($group_name);
		
		$statusCode = 200;
		
		if($rawData)
			$rawData = "true";
		else
			$rawData = "false";
		
		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
				
		echo $rawData;
	}
	
	public function joinGroup($group_id,$user_id) {

		$group = new Group();
		$rawData = $group->joinGroup($group_id,$user_id);

		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No group name found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
		echo $rawData;	
		/*if(strpos($requestContentType,'application/json') !== false){
			echo "result === ".$rawData;
			$response = $this->encodeJson($rawData);
			echo $response;
		} else if(strpos($requestContentType,'text/html') !== false){
			$response = $this->encodeHtml($rawData);
			echo $response;
		} else if(strpos($requestContentType,'application/xml') !== false){
			$response = $this->encodeXml($rawData);
			echo $response;
		}*/
	}
	
	public function InsertGroupScore($group_id,$score) {

		$group = new Group();
		$rawData = $group->insertGroupScore($group_id,$score);

		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No group name found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
		echo $rawData;	
	}
	
	public function getGroupScore($group_id) {

		$group = new Group();
		$rawData = $group->getGroupScore($group_id);

		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No group name found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
		echo $rawData;	
	}
	
	public function getTopGroupScore() {

		$group = new Group();
		$rawData = $group->getTopGroupScore();

		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No group name found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
		echo $rawData;	
	}
}
?>