<?php

Class User {
	
	public function createUser($name, $email, $city, $state, $facebookId){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		 
		$sql="INSERT INTO `contagotas_app`.`users`
		(`name`,
		`email`,
		`city`,
		`state`,
		`facebook_id`)
		VALUES 
		('" . $name . "',
		'" . $email . "',
		'" . $city . "',
		'" . $state . "',
		'" . $facebookId .
		"')";
		
		$result = mysqli_query($con,$sql);
		
		if (!$result)
		  {
		  die('Error creating user: ' . mysqli_error($con));
		  }
		  
		$sql="SELECT *
			FROM `contagotas_app`.`users`
			where `users`.`name` = '" . $name . "';";
		
		$result = mysqli_query($con,$sql);
		
		if (!$result)
		  {
		  die('Error selecting user ID: ' . mysqli_error($con));
		  }
		
		
		while($row = $result->fetch_assoc()) {
			$user_id = $row["userid"];
		}
			
		mysqli_close($con);
		return $user_id;
	
	}
	
	public function insertUserScore($user_id,$score){

		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");

		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		
		
		$sql = "INSERT INTO `contagotas_app`.`user_score` 
		(`userid`,`score`) VALUES
			(".$user_id.",
			".$score.")";
		
		if (!mysqli_query($con,$sql))
		  {
		  die('Error: ' . mysqli_error($con));
		  }
		
		if(mysqli_affected_rows($con) > 0)
			$return = "success";
		else
			$return = "ERROR - 0 rows affected_rows";
		
		mysqli_close($con);
		return $return;
	}
	
	public function getUserScore($user_id){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	
		$sql = "SELECT contagotas_app.users.name, IFNULL(SUM(contagotas_app.user_score.score), 0) as score 
		FROM contagotas_app.users
		LEFT OUTER  JOIN contagotas_app.user_score ON contagotas_app.users.userid=contagotas_app.user_score.userid
        where contagotas_app.user_score.userid = '" . $user_id . "'";
		
		$result = $con->query($sql);
		
		if (!mysqli_query($con,$sql))
		  {
		  die('Error: ' . mysqli_error($con));
		  }
		 
		$return = "";

		if ($result->num_rows > 0) {
			// output data of each row
			while($row = $result->fetch_assoc()) {
				$return = "[{'playerName':'" . $row["name"]."','playerPoints':'" . $row["score"] . "'}]";
			}
		} 
			
		mysqli_close($con);
		
		return $return;
	}
	
	public function getUserRanking(){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	 
		$sql = 'SELECT contagotas_app.users.name, IFNULL(SUM(contagotas_app.user_score.score), 0) as score 
		FROM contagotas_app.users
		LEFT OUTER  JOIN contagotas_app.user_score ON contagotas_app.users.userid=contagotas_app.user_score.userid
        group by contagotas_app.users.name
		order by score DESC limit 10;';

		$result = $con->query($sql);

		$json = "[";
		if ($result->num_rows > 0) {
			// output data of each row
			while($row = $result->fetch_assoc()) {
				$json .= "{'playerName':'" . $row["name"]."','playerPoints':'" . $row["score"] . "'},";
			}
		} 
		$json .= "]";
		
		if (!mysqli_query($con,$sql))
		  {
		  die('Error: ' . mysqli_error($con));
		  }
		 
		mysqli_close($con);
		return $json;
	}
}