<?php

include_once 'settings.php';

$sql = "SELECT posX, posY, posZ FROM Death";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  while($row = $result->fetch_assoc()) {
    echo "" . $row["posX"]. " " . $row["posY"]. " " . $row["posZ"]. "<br>";
  }
} else {
  echo "0 results";
}
$conn->close();

?>