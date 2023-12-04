open FSharp.Data
open FSharp.Data.HttpRequestHeaders

type RondonopolisXsd = XmlProvider<Schema="./nfse-v-100.xsd">

type CpfCnpj = RondonopolisXsd.CpfCnpj

type IdentificacaoPrestador = RondonopolisXsd.IdentificacaoPrestador

type IdentificacaoNfse = RondonopolisXsd.IdentificacaoNfse

type PedidoCancelamento = RondonopolisXsd.PedidoCancelamento

type CancelarNfseEnvio = RondonopolisXsd.CancelarNfseEnvio

type CancelarNfseResposta = RondonopolisXsd.CancelarNfseResposta

let createCpf cpf = CpfCnpj(cnpj = None, cpf = Some cpf)

let createCnpj cnpj = CpfCnpj(cnpj = Some cnpj, cpf = None)

let unidadeGestora = "1"

let cpfCnpj = createCnpj "123"

let identificacaoPrestador: IdentificacaoPrestador =
    IdentificacaoPrestador(chaveDigital = Some "123", signature = None, cpfCnpj = cpfCnpj, inscricaoMunicipal = None)

let identificacaoNfse = IdentificacaoNfse("123", identificacaoPrestador)

let pedidoCancelamento =
    PedidoCancelamento(identificacaoNfse, "codigoCancelamento", "justificativaCancelamento", "1.00")

let cancelarNfseEnvio = CancelarNfseEnvio(unidadeGestora, pedidoCancelamento)


let xml = cancelarNfseEnvio.XElement.ToString()

printfn 
    """
Request: 
%s
    """ xml


let handleResponse (resp: CancelarNfseResposta option) =
    resp
    |> Option.bind _.ListaMensagemRetorno
    |> Option.iter (_.ToString() >> printfn 
    """
Response: 
%s
    """)

let sendRequest = 
    Http.Request("https://nfse.rondonopolis.mt.gov.br/api/CancelarNfse", 
        body = TextRequest (cancelarNfseEnvio.XElement.ToString()),
        headers = [ ContentType HttpContentTypes.Xml; Accept HttpContentTypes.Xml ],
        silentHttpErrors = true) 

let mapToTextContent resp =
    match resp with
    | Text text -> text
    | _ -> ""

sendRequest
    |> _.Body
    |> mapToTextContent
    |> RondonopolisXsd.Parse
    |> _.CancelarNfseResposta
    |> handleResponse
