<?php
require_once("SimpleRest.php");
require_once("User.php");
		
class UserRestHandler extends SimpleRest {

	function createUser($name, $email, $city, $state, $facebookId) {	
		
		$user = new User();
		$rawData = $user->createUser($name, $email, $city, $state, $facebookId);

		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No user found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
				
		echo $rawData;
	}	
	
	public function insertScore($user_id,$score) {

		$user = new User();
		$rawData = $user->insertUserScore($user_id,$score);

		$response = new Response();
		$response->send($rawData);
	}
	
	public function getUserScore($user_id) {

		$user = new User();
		$rawData = $user->getUserScore($user_id);

		$response = new Response();
		$response->send($rawData);
	}
	
	public function getUserRanking() {

		$user = new User();
		$rawData = $user->getUserRanking();

		$response = new Response();
		$response->send($rawData);
	}
}
?>