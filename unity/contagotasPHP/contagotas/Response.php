<?php
require_once("SimpleRest.php");

class Response extends SimpleRest {
	
	public function send(string $rawData)
	{
		if(empty($rawData)) {
			$statusCode = 404;
			$rawData = array('error' => 'No group name found!');		
		} else {
			$statusCode = 200;
		}

		$requestContentType = $_SERVER['HTTP_ACCEPT'];
		$this ->setHttpHeaders($requestContentType, $statusCode);
		echo utf8_decode ($rawData);	
	}
	
}
?>
