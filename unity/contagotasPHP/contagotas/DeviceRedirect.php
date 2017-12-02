<?php

$ipadLink = "http://www.uol.com.br";
$androidLink = "http://www.globo.com.br";

//Detect special conditions devices
$iPod    = stripos($_SERVER['HTTP_USER_AGENT'],"iPod");
$iPhone  = stripos($_SERVER['HTTP_USER_AGENT'],"iPhone");
$iPad    = stripos($_SERVER['HTTP_USER_AGENT'],"iPad");
$Android = stripos($_SERVER['HTTP_USER_AGENT'],"Android");


//do something with this information
if( $iPod || $iPhone ){
    header("Location: " . $ipadLink);
die();
}else if($iPad){
    header("Location: " . $ipadLink);
}else if($Android){
    header("Location: " . $androidLink);
}

?> 