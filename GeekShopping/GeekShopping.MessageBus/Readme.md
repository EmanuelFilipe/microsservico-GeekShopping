# para baixar e rodar o rabbitmq:
	abra o powershell e cole esse código: 
	docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management

	comando só para rodar a imagem:
	docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:3-management

# acesse o rabbitmq pelo endereço -> no firefox: 
	http://localhost:15672/
	senha e login: guest

	no chrome: 127.0.0.1:15672

# adicione o RabbitMQ.Client via nugget na versão 6.2.2
