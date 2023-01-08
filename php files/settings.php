<?php
    $servername='localhost';
    $username='oscarta3';
    $password='fNut3jDHvy';
    $dbname = "oscarta3";
    $conn=mysqli_connect($servername,$username,$password,"$dbname");
      if(!$conn){
          die('Could not Connect MySql Server:' .mysql_error());
        }
      if($conn){
            echo('Success connecting to database! ');
        }
?>