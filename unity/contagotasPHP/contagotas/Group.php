<?php

Class Group {

	public function getAllGroup(){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	 
		$sql = 'SELECT * FROM contagotas_app.group ORDER BY id_group DESC LIMIT 20';
		$result = $con->query($sql);

		if ($result->num_rows > 0) {
			// output data of each row
			$json = "[";
			while($row = $result->fetch_assoc()) {
				$json .= "{'id':'" . $row["id_group"]."','Name':'" . $row["group_name"] . "'},";
			}
			$json .= "]";
		} 
		else {
			echo "0 results";
		}

		if (!mysqli_query($con,$sql))
		  {
		  die('Error: ' . mysqli_error($con));
		  }
		 
		mysqli_close($con);
		return $json;
	}
	
	public function getGroup($group_name_query){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	 
		$sql = "SELECT * FROM contagotas_app.group where group_name = '" . $group_name_query . "'";
		$result = $con->query($sql);
		
		$json = "[";
		if ($result->num_rows > 0) {
			// output data of each row
		
			while($row = $result->fetch_assoc()) {
				$json .= "{'id':'" . $row["id_group"]."','Name':'" . $row["group_name"] . "'},";
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
	
	public function createGroup($name){

		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		 
		$sql="INSERT INTO `contagotas_app`.`group`
			(`group_name`)
			VALUES
			('" . $name . "');";
		
		$result = mysqli_query($con,$sql);
		
		if (!$result)
		  {
		  die('Error creating group: ' . mysqli_error($con));
		  }
		  
		$sql="SELECT *
			FROM `contagotas_app`.`group`
			where `group`.`group_name` = '" . $name . "';";
		
		$result = mysqli_query($con,$sql);
		
		if (!$result)
		  {
		  die('Error selecting group ID: ' . mysqli_error($con));
		  }
		
		
		while($row = $result->fetch_assoc()) {
			$group_id = $row["id_group"];
		}
			
		mysqli_close($con);
		return $group_id;
	}
	
	public function hasGroup($user){

		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		
		
		$sql= "SELECT * FROM contagotas_app.group_users where user_id = " . $user . ";";
		
		$result = mysqli_query($con,$sql);
		
		if (!$result)
		{
		  die('Error: ' . mysqli_error($con));
		}
			
		mysqli_close($con);
		return $result->num_rows > 0;
	}
	
	public function joinGroup($group,$user){

		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		 
		$sql="INSERT INTO `contagotas_app`.`group_user`
		(`user_id`,`group_id`)	
		VALUES 	('" . $user . "','" . $group . "');";
		 
		if (!mysqli_query($con,$sql))
		  {
		  die('Error: ' . mysqli_error($con));
		  }
		 
		mysqli_close($con);
		return "sucess";
	}
	
	public function LeaveGroup($user){

		//$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		$mysqli = new mysqli("mysql.contagotas.online", "contagotas", "c0nt4g0t4s", "contagotas_app");

		/* check connection */
		if (mysqli_connect_errno()) {
			printf("Connect failed: %s\n", mysqli_connect_error());
			exit();
		}
		 
		$mysqli->query("DELETE FROM Language WHERE Percentage < 50");
		 
		
		
		$con=mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s","contagotas_app");
		if (mysqli_connect_errno())
		  {
		  echo "Failed to connect to MySQL: " . mysqli_connect_error();
		  }

		mysqli_query($con,"DELETE FROM `contagotas_app`.`group_user` WHERE user_id = " . $user .";");

		if(mysqli_affected_rows($con) > 0)
			$return = "sucess";
		else
			$return = "ERROR - 0 rows affected_rows";
		
		mysqli_close($con);
		
		return $return;
	}

}
?>