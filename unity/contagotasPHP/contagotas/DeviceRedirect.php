<?php

$ipadLink = "https://play.google.com/store/apps/details?id=br.gov.caixa.contagotas&hl=pt_BR";
$androidLink = "https://play.google.com/store/apps/details?id=br.gov.caixa.contagotas&hl=pt_BR";

//Detect special conditions devices
$iPod    = stripos($_SERVER['HTTP_USER_AGENT'],"iPod");
$iPhone  = stripos($_SERVER['HTTP_USER_AGENT'],"iPhone");
$iPad    = stripos($_SERVER['HTTP_USER_AGENT'],"iPad");
$Android = stripos($_SERVER['HTTP_USER_AGENT'],"Android");


//do something with this information
if( $iPod || $iPhone ){
    header("Location: " . $ipadLink);
}else if($iPad){
    header("Location: " . $ipadLink);
}else if($Android){
    header("Location: " . $androidLink);
}
else
	Echo "another device";
?> 