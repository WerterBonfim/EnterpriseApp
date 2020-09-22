### Melhorias:

Client Rest: 
https://github.com/reactiveui/refit
https://github.com/tmenier/Flurl





### Estrutura

NerdStoreEnterprise
src/
	Building Blocks
		Core/NSE.Core  (ClassLibrary)
	Services
        Carrinho/NSE.Carrinho.API
            
        Catalogo/NSE.Catalogo.API
        Cliente/NSE.Cliente.API        
		Identidade/NSE.Identidade.API
        Dependencias: [
                Microsoft.AspNetCore.Authentication. JwtBearer 3.1.8
                Microsoft.AspNetCore.Identity.EntityFrameworkCore 3.1.3
                Microsoft.AspNetCore.Identity.UI 3.1.3
                Microsoft.EntityFrameworkCore 3.1.8
                Microsoft.EntityFrameworkCore.Abstractions
                Microsoft.EntityFrameworkCore.Relational
                Microsoft.EntityFrameworkCore.SqlServer
                Microsoft.EntityFrameworkCore.Tools
                Swashbuckle.AspNetCore

            ]
        Pagamento/NSE.Pagamento.API
        Pedido/NSE.Pedido.API        
	Web
		MVC/NSE.WebApp.MVC (WebApplication Model-ViewController, Enable Razor) 
        Dependencias: [
                Microsoft.AspNetCore.Authentication. JwtBearer 3.1.8
               

            ]
