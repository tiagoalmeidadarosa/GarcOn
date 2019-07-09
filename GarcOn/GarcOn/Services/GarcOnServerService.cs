﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IGarcOn")]
public interface IGarcOn
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGarcOn/GetData", ReplyAction="http://tempuri.org/IGarcOn/GetDataResponse")]
    string GetData();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGarcOn/AddCategory", ReplyAction="http://tempuri.org/IGarcOn/AddCategoryResponse")]
    void AddCategory(int tipo, string descricao);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGarcOn/AddProduct", ReplyAction="http://tempuri.org/IGarcOn/AddProductResponse")]
    void AddProduct(long idCategoria, string nome, string descricao, double valor, byte[] foto, long[] adicionais);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGarcOn/AddOrder", ReplyAction="http://tempuri.org/IGarcOn/AddOrderResponse")]
    string AddOrder(int mesa, double valorTotal, string itensPedido);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGarcOn/AddAccountRequest", ReplyAction="http://tempuri.org/IGarcOn/AddAccountRequestResponse")]
    string AddAccountRequest(int mesa, double valorTotal, string sugestao);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGarcOn/AddKitchenOrder", ReplyAction="http://tempuri.org/IGarcOn/AddKitchenOrderResponse")]
    string AddKitchenOrder(long idPedido, int status);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGarcOn/AddAditional", ReplyAction="http://tempuri.org/IGarcOn/AddAditionalResponse")]
    void AddAditional(string descricao, double valor);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IGarcOnChannel : IGarcOn, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class GarcOnClient : System.ServiceModel.ClientBase<IGarcOn>, IGarcOn
{
    
    public GarcOnClient()
    {
    }
    
    public GarcOnClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public GarcOnClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public GarcOnClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public GarcOnClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public string GetData()
    {
        return base.Channel.GetData();
    }
    
    public void AddCategory(int tipo, string descricao)
    {
        base.Channel.AddCategory(tipo, descricao);
    }
    
    public void AddProduct(long idCategoria, string nome, string descricao, double valor, byte[] foto, long[] adicionais)
    {
        base.Channel.AddProduct(idCategoria, nome, descricao, valor, foto, adicionais);
    }
    
    public string AddOrder(int mesa, double valorTotal, string itensPedido)
    {
        return base.Channel.AddOrder(mesa, valorTotal, itensPedido);
    }
    
    public string AddAccountRequest(int mesa, double valorTotal, string sugestao)
    {
        return base.Channel.AddAccountRequest(mesa, valorTotal, sugestao);
    }
    
    public string AddKitchenOrder(long idPedido, int status)
    {
        return base.Channel.AddKitchenOrder(idPedido, status);
    }
    
    public void AddAditional(string descricao, double valor)
    {
        base.Channel.AddAditional(descricao, valor);
    }
}
