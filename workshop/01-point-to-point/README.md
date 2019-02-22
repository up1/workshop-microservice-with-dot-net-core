## Step 1 :: Start service

```
$cd EmployeeApi
$docker image build -t point-to-point .
$docker container run -d -p 8080:80 --name employee point-to-point
```

Open url=http://localhost:8080/ in browser

## Step 2 :: Call service from client
```
$docker image build -t employee-client .
$docker container run --rm --link employee:employee employee-client:latest
```
