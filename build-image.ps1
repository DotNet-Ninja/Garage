$tag = "0.0.1-alpha"

$vTag = "dotnetninjax/garage:$tag"
$lTag = "dotnetninjax/garage:latest"

docker build . --tag $vTag
docker tag  $vTag $lTag

docker push $vTag
docker push $lTag