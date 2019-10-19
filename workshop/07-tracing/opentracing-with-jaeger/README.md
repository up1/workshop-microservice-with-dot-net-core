# OpenTracing with jaeger

## Step 1 :: Start [Jaeger server](https://www.jaegertracing.io/)
```
$docker run -d --name jaeger \
  -e COLLECTOR_ZIPKIN_HTTP_PORT=9411 \
  -p 5775:5775/udp \
  -p 6831:6831/udp \
  -p 6832:6832/udp \
  -p 5778:5778 \
  -p 16686:16686 \
  -p 14268:14268 \
  -p 9411:9411 \
  jaegertracing/all-in-one:1.8
```

Open url=http://localhost:16686 in browser

## Step 2 :: Start Service B
```
$docker image build -t service_b -f Dockerfile_b .
$docker container run -d --name service_b -p 8082:80 --link jaeger:jaeger service_b
```

Open url=http://localhost:8082/bworld/id/1 in browser

## Step 3 :: Start Service A
```
$docker image build -t service_a -f Dockerfile_a .
$docker container run -d -p 8081:80 --link service_b:service_b --link jaeger:jaeger service_a
```

Open url=http://localhost:8081/aworld/id/1 in browser
