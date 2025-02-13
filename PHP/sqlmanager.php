<?php
$servername = "localhost";
$username = "";
$password = "";
$dbname = "";

$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_POST['login']) && isset($_POST['password'])) {
        $login = $_POST['login'];
        $password = $_POST['password'];

        if (empty($login) || empty($password)) {
            die('error');
        }

        $sql = "SELECT * FROM `accounts` WHERE `login` = '". $login."'";
        
        $result = $conn->query($sql);

        if ($result->num_rows > 0) {
            while($row = $result->fetch_assoc()) {
                $pass = password_verify($_POST['password'], $row["password"]);
                
                if ($pass == true ) {
                    $sql = "SELECT * FROM `bans` WHERE `bannedUserId` = '". $row["id"]."'";
        
                    $result = $conn->query($sql);

                    if ($result->num_rows > 0) {
                        while($row = $result->fetch_assoc()) {
                            echo "banned|". $row["reason"]. "|". $row["unbanDate"];
                        }
                    }else{
                        echo $row["id"]. "|". $row["UpgradesSave"];
                    }
                    
                }else {
                    echo "wrong password";
                }
            }
        }
        
    }
    elseif (isset($_POST['sqlPos']) && isset($_POST['toPrint'])) {
        $sql = $_POST['sqlPos'];
        $toPrint = $_POST['toPrint'];
        
        $result = $conn->query($sql);
        if ($result) {
            $row = $result->fetch_assoc();
            echo $row[$toPrint];
        } else {
            echo "error";
        }
    }
}
?>