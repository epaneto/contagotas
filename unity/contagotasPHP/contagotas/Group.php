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
		
		
		$sql = "SELECT * FROM contagotas_app.group where group_name like '%" . $group_name . "%'";
		$result = $con->query($sql);
		
		$json = "[";
		if ($result->num_rows > 0) {
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
	
	public function getGroupByUserId($user_id){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		
		$sql = "SELECT contagotas_app.group.group_name, contagotas_app.group.id_group, IFNULL(SUM(contagotas_app.group_score.score), 0) as score
				FROM contagotas_app.group
				INNER JOIN contagotas_app.group_user ON contagotas_app.group.id_group=contagotas_app.group_user.group_id
				INNER JOIN contagotas_app.group_score ON contagotas_app.group_score.group_id=contagotas_app.group.id_group
				WHERE contagotas_app.group_user.user_id = '" . $user_id . "'";
				

		$result = $con->query($sql);
		
		$json = "[";
		if ($result->num_rows > 0) {
			// output data of each row
		
			while($row = $result->fetch_assoc()) {
				$json .= "{'id':'" . $row["id_group"]."','Name':'" . $row["group_name"] . "','Score':'" . $row["score"] . "'}";
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
	
	public function createGroup($name, $password){
	
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		 
		$sql="INSERT INTO `contagotas_app`.`group`
			(`group_name`,`password`)
			VALUES
			('" . $name . "', ENCRYPT('" . $password . "', 'contagotas-app-secure-key'));";
		
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
	
	public function joinGroupWithPassword($group,$user,$password){

		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		
		
		$sql= "SELECT * FROM contagotas_app.group where contagotas_app.group.id_group = '" . $group ."' AND password = ENCRYPT('" . $password . "', 'contagotas-app-secure-key');";

		$result = mysqli_query($con,$sql);
		
		if (!$result)
		{
		  die('Error: ' . mysqli_error($con));
		}
			
		if( $result->num_rows == 0 )
		{
			return "wrong password";
		}
		
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
			$return = "success";
		else
			$return = "ERROR - 0 rows affected_rows";
		
		mysqli_close($con);
		
		return $return;
	}
	
	public function InsertGroupScore($group,$score, $user_id){

		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");

		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		
		
		$sql = "INSERT INTO `contagotas_app`.`group_score` 
		(`group_id`,`score`,`user_id`) VALUES
			(".$group.",
			".$score.",
			".$user_id.")";
		
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
	
	public function getGroupScore($group_id){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	 
		$sql = "SELECT IFNULL(SUM(score), 0) as score FROM contagotas_app.group_score where group_id = '" . $group_id . "'";
		
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
		 	 
		$sql = 'SELECT contagotas_app.group.group_name, IFNULL(SUM(contagotas_app.group_score.score), 0) as score 
		FROM contagotas_app.group
		LEFT OUTER  JOIN contagotas_app.group_score ON contagotas_app.group.id_group=contagotas_app.group_score.group_id
        group by contagotas_app.group.group_name
		order by score DESC limit 8;';

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
	
	public function getGroupScoreDetailed($group_id){
		
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		
		$sql = "SELECT contagotas_app.users.name as name , IFNULL(SUM(contagotas_app.group_score.score), 0) as score 
		FROM contagotas_app.group_user
		inner JOIN contagotas_app.users ON contagotas_app.group_user.user_id = contagotas_app.users.userid 
		LEFT OUTER JOIN contagotas_app.group_score ON contagotas_app.group_score.group_id = contagotas_app.group_user.group_id
		where group_user.group_id = " . $group_id ."
		GROUP BY contagotas_app.users.name";

		$result = $con->query($sql);

		$json = "[";
		if ($result->num_rows > 0) {
			// output data of each row
			while($row = $result->fetch_assoc()) {
				$json .= "{'Name':'" . $row["name"]."','score':'" . $row["score"] . "'},";
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
	
	public function createInvite($inviter_id , $facebook_id){

		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");		 
		
		$sql="INSERT INTO `contagotas_app`.`group_invite`
			(`group_id`,
			`user_id`,
			`inviter_id`)
			VALUES
			( (SELECT group_id FROM contagotas_app.group_user where user_id = '" . $inviter_id. "' LIMIT 1) ,
			  (SELECT userid FROM contagotas_app.users where facebook_id = '" . $facebook_id. "' LIMIT 1),
			'" . $inviter_id . "');";

		$result = $con->query($sql);

		if (!$result)
		{
			die('Error: ' . mysqli_error($con));
		}
		 
		return 'success';	
	}
	
	public function checkInvite($user_id){
				
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	 
		$sql="SELECT contagotas_app.group_invite.id_invite, contagotas_app.group.group_name, contagotas_app.users.name
		FROM contagotas_app.group_invite
		INNER JOIN contagotas_app.group ON contagotas_app.group_invite.group_id=contagotas_app.group.id_group
		INNER JOIN contagotas_app.users ON contagotas_app.group_invite.user_id=contagotas_app.users.userid
		WHERE contagotas_app.group_invite.user_id = '" . $user_id . "' AND contagotas_app.group_invite.verified = false;";
		$result = $con->query($sql);
		
		$json = "[";
		if ($result->num_rows > 0) {
			// output data of each row
		
			while($row = $result->fetch_assoc()) {
				$json .= "{'id_invite':'" . $row["id_invite"]. "','group_name':'" . $row["group_name"]."','sender_name':'" . $row["name"] . "'}";
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
	
	public function acceptInvite($invite_id){
				
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		
		$return_data;
		try {
			$con->begin_transaction();

			$con->query("SET @invite_id = '" . $invite_id. "';");
			$con->query("SET @group_id = (SELECT contagotas_app.group_invite.group_id FROM contagotas_app.group_invite WHERE contagotas_app.group_invite.id_invite = @invite_id);");
			$con->query("SET @user_id = (SELECT contagotas_app.group_invite.user_id FROM contagotas_app.group_invite WHERE contagotas_app.group_invite.id_invite = @invite_id);");
			$con->query("INSERT INTO `contagotas_app`.`group_user` (`group_id`,`user_id`) VALUES (@group_id,@user_id);");
			$con->query("UPDATE `contagotas_app`.`group_invite` SET `verified` = true WHERE `id_invite` = @invite_id;");
			$result = $con->query("SELECT  `contagotas_app`.`group`.id_group, `contagotas_app`.`group`.group_name FROM contagotas_app.group WHERE id_group = @group_id;");
			
			$con->commit();
			
			$json = "[";
			if ($result->num_rows > 0) {
				
				while($row = $result->fetch_assoc()) {
					$json .= "{'id':'" . $row["id_group"]."','Name':'" . $row["group_name"] . "'},";
				}
				
			} 
			$json .= "]";
			$return_data = $json;
			
		} catch (Exception $e) {
			$con->rollback();
			die('Error: ' . mysqli_error($con));
		}
	 
		return $json;
	}
	
	public function denyInvite($invite_id){
				
		$con = mysqli_connect("mysql.contagotas.online","contagotas","c0nt4g0t4s");
		
		if (!$con)
		{
		  die('Could not connect: ' . mysqli_error($con));
		}
		 
		mysqli_select_db($con,"contagotas_app");
		 	 
		$sql="	UPDATE `contagotas_app`.`group_invite` SET `verified` = true WHERE `id_invite` = " . $invite_id . ";";
		$result = $con->query($sql);
		
		if (!$result)
		{
			die('Error: ' . mysqli_error($con));
		}
		 
		return 'success';	
	}
}
?>