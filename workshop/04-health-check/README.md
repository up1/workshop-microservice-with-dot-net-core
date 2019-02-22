## Step 1 :: Start service registry (consul)
```
$docker container run --rm --name=consul -e CONSUL_BIND_INTERFACE=eth0 -e CONSUL_CLIENT_INTERFACE=eth0 -p 8500:8500 consul
```

Open url=http://localhost:8500 in browser

## Step 2 :: Start service

```
$cd EmployeeApi
$docker image build -t point-to-point .
$docker container run --rm -p 8080:80 --name employee --link consul:consul point-to-point
```

Open url=http://localhost:8080/ in browser

## Step 3 :: Call service from client
```
$cd EmployeeClient
$docker image build -t employee-client .
$docker container run --rm --link consul:consul --link employee:employee employee-client:latest
```
