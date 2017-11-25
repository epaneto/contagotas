<?php

Class User {
	
	public function createUser($name, $email){
		
		echo "receiving";
		echo "name = ".$name;
		echo "email = ".$email;
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		 
		$sql="INSERT INTO `contagotas_app`.`users`
		(`name`,
		`email`)
		VALUES 
		('" . $name . "',
		'" . $email . "')";
		
		$result = mysqli_query($con,$sql);
		
		if (!$result)
		  {
		  die('Error creating group: ' . mysqli_error($con));
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
			$group_id = $row["userid"];
		}
			
		mysqli_close($con);
		return $group_id;
	
	}
}