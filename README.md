Avaliação Banco Bari

Um exemplo básico de comunicação entre entre Microserviços

BancoBari.Consumer é a api que realiziza o consumo de mensagens da api BancoBari.Produce, responsavel por produzir as mensagens. O procedimento de envio e recebimento pode ser invertido tranquilamente entre as apis, visto que a rotina de mensageria foi construida em uma camada desacoplada, facilitando a implementação.

A aplicação está configurada para rodar via Docker, só precisa gerar a imagem e o container na maquina que for realizar o teste e inicar o procedimento.


