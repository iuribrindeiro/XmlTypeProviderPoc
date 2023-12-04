Type Provider is a power feature from F# that allows you to dynamically use types based on a URL, File, String content, or anything that defines a type schema.

Once you reference that type schema you can just get the result of the type provider and use it as any other type.

Example: 

```fsharp
type RondonopolisXsd = XmlProvider<Schema="./nfse-v-100.xsd">

let cpfCnpj = RondonopolisXsd.CpfCnpj(cnpj = None, cpf = Some "123")

printfn "%s" cpfCnpj.Cpf.Value
```

This will print "123".

Once you have these instances based on these provided types, you can just call ```cpfCnpj.XElement.ToString()``` and you will have the formatted XML.
