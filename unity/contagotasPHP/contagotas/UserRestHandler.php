<?php
require_once("SimpleRest.php");
require_once("User.php");
		
class UserRestHandler extends SimpleRest {

	function createUser($name, $email, $city, $state, $facebookId) {	
		
		$group = new User();
		$rawData = $group->createUser($name, $email, $city, $state, $facebookId);

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
}
?>