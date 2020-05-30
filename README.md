## Léxico del lenguaje aceptado por el compilador 

Un sensor de temperatura tiene la capacidad de constantemente de enviar datos a un centro de control para ser interpretados. De igual manera, tiene la capacidad de recibir instrucciones, que él interpretará. El sensor trabaja en el rango de -7 y +7. Funciona de la siguiente manera:

1. ### Envío de datos desde el sensor hacia el centro de control
    - Respuesta del sensor con la temperatura actual:
        - OUT#<valor_retorno>#<unidad_de_medida>
    - Respuesta del sensor con estado operativo sin problema:
        - OUT#SUCCESS#ON
    - Respuesta del sensor con estado operativo, pero con fallas:
        - OUT#FAIL#ON
    - Respuesta del sensor con estado apagado:
        - OUT#OFF

2. ### Envío de instrucciones desde el centro de control al sensor
    - Solicitar aumentar temperatura:
        - IN#< valor_envio>#<unidad_de_medida>#UP
    - Solicitar disminuir temperatura:
        - IN#<valor_envio >#<unidad_de_medida>#DOWN
    - Solicitar consultar temperatura actual:
        - IN#<unidad_de_medida>#GET 
    - Solicitar encender el sensor:
        - IN#START
    - Solicitar apagar el sensor:
        - IN#SHUTDOWN
    - Solicitar reiniciar el sensor:
        - IN#RESTART
    - Solicitar estado sensor:
        - IN#STATUS


### ANEXOS

* <valor_retorno>:  4 dígitos consecutivos binarios, que corresponden a un número entero con signo, de la siguiente manera:

|SIGNO|4|2|1| NÚMERO
|----------|:-----------:|:------:|:----:|:----|
|0|0|0|0|0|
|0|0|0|1|-1|
|0|0|1|0|-2|
|0|0|1|1|-3|
|0|1|0|0|-4|
|0|1|0|1|-5|
|0|1|1|0|-6|
|0|1|1|1|-7|
|1|0|0|0|0|
|1|0|0|1|1|
|1|0|1|0|2|
|1|0|1|1|3|
|1|1|0|0|4|
|1|1|0|1|5|
|1|1|1|0|6|
|1|1|1|1|7|

* <valor_envio>: 3 dígitos consecutivos binarios, que corresponden a un número entero positivo, de la siguiente manera:

|4|2|1| NÚMERO
|:-----------:|:------:|:----:|:----|
|0|0|0|0|
|0|0|1|1|
|0|1|0|2|
|0|1|1|3|
|1|0|0|4|
|1|0|1|5|
|1|1|0|6|
|1|1|1|7|

* <unidad_de_medida>: Corresponde a la unidad de medida de la temperatura

|LETRA|SIGNIFICADO|
|:-----------:|:------:|
|C|Centígrados|
|F|Farenheit|
|K|Kelvin|
|R|Rankine|




## AUTOMÁTA


