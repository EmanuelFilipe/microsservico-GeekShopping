#para baixar e rodar o rabbitmq:
	abra o powershell e cole esse código: 
	docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management

#acesse o rabbitmq pelo endereço: http://localhost:15672/
senha e login: guest

# adicione o RabbitMQ.Client via nugget na versão 6.2.2

# a ordem de execução é:
cartapi
couponapi
identityserver
productapi
web