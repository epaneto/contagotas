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
	
	public function getGroup($group_id){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	 
		$sql = "SELECT * FROM contagotas_app.group where id_group = '" . $group_id . "'";
		$result = $con->query($sql);
		
		$json = "[";
		if ($result->num_rows > 0) {
			// output data of each row
		
			while($row = $result->fetch_assoc()) {
				$json .= "{'id':'" . $row["id_group"]."','Name':'" . $row["group_name"] . "'}";
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
	
	public function getGroupByName($group_name){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	 
		$sql = "SELECT * FROM contagotas_app.group where group_name = '" . $group_name . "'";
		$result = $con->query($sql);
		
		$json = "[";
		if ($result->num_rows > 0) {
			while($row = $result->fetch_assoc()) {
				$json .= "{'id':'" . $row["id_group"]."','Name':'" . $row["group_name"] . "'}";
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
	
	public function getGroupByUserId($user_id){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		
		$sql = "SELECT contagotas_app.group.group_name, contagotas_app.group.id_group
				FROM contagotas_app.group
				INNER JOIN contagotas_app.group_user ON contagotas_app.group.id_group=contagotas_app.group_user.group_id
				WHERE contagotas_app.group_user.user_id = '" . $user_id . "'";

		$result = $con->query($sql);
		
		$json = "[";
		if ($result->num_rows > 0) {
			// output data of each row
		
			while($row = $result->fetch_assoc()) {
				$json .= "{'id':'" . $row["id_group"]."','Name':'" . $row["group_name"] . "'}";
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
		
		
		$sql= "SELECT * FROM contagotas_app.group_user where user_id = " . $user . ";";
		
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
		 
		$result = $con->query($sql);

		if (!$result)
		{
			die('Error: ' . mysqli_error($con));
		}
		 
		mysqli_close($con);
		$GroupClass = new Group();
		return $GroupClass->getGroup($group);		
	}
	
	public function LeaveGroup($user){

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
	
	public function InsertGroupScore($group,$score){

		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");

		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		
		$sql = " UPDATE `contagotas_app`.`group_score` 
			SET score = score + " . $score . "
			WHERE group_id = '".$group."'";
		
		if (!mysqli_query($con,$sql))
		  {
		  die('Error: ' . mysqli_error($con));
		  }
		
		if(mysqli_affected_rows($con) > 0)
			$return = "sucess";
		else
			$return = "ERROR - 0 rows affected_rows";
		
		mysqli_close($con);
		return $return;
	}
	
	public function getGroupScore($group_id){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	 
		$sql = "SELECT score FROM contagotas_app.group_score where group_id = '" . $group_id . "'";
		
		$result = $con->query($sql);
		
		if (!mysqli_query($con,$sql))
		  {
		  die('Error: ' . mysqli_error($con));
		  }
		 
		$return = "";

		if ($result->num_rows > 0) {
			// output data of each row
			while($row = $result->fetch_assoc()) {
				$return = $row["score"];
			}
		} 
		
		mysqli_close($con);
		
		return $return;
	}	

	public function getTopGroupScore(){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	 
		$sql = 'SELECT contagotas_app.group.group_name, contagotas_app.group_score.score
		FROM contagotas_app.group
		INNER JOIN contagotas_app.group_score ON contagotas_app.group.id_group=contagotas_app.group_score.group_id
		order by score DESC limit 10;';

		$result = $con->query($sql);

		$json = "[";
		if ($result->num_rows > 0) {
			// output data of each row
			while($row = $result->fetch_assoc()) {
				$json .= "{'Name':'" . $row["group_name"]."','score':'" . $row["score"] . "'},";
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
?>