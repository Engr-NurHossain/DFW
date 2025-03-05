namespace HS.Web.UI.Models
{
    using System;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.Xml;

    public class MessageBehavior : IEndpointBehavior
    {
        string _username;
        string _password;

        public MessageBehavior(string username, string password)
        {
            _username = username;
            _password = password;
        }

        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        { }

        void IEndpointBehavior.ApplyClientBehavior(System.ServiceModel.Description.ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new MessageInspector(_username, _password));
        }
        void IEndpointBehavior.ApplyDispatchBehavior(System.ServiceModel.Description.ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        { }
        void IEndpointBehavior.Validate(System.ServiceModel.Description.ServiceEndpoint endpoint)
        { }

    }

    public class MessageInspector : IClientMessageInspector
    {
        string _username;
        string _password;

        public MessageInspector(string username, string password)
        {
            _username = username;
            _password = password;
        }


        void IClientMessageInspector.AfterReceiveReply(ref System.ServiceModel.Channels.Message reply,
            Object correlationState)
        {
        }

        object IClientMessageInspector.BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            request.Headers.Clear();
            string headerText = "<wsse:UsernameToken xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">" +
                                    "<wsse:Username>{0}</wsse:Username>" +
                                    "<wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">" +
                                    "{1}</wsse:Password>" +
                                "</wsse:UsernameToken>";

            headerText = string.Format(headerText, _username, _password);

            XmlDocument MyDoc = new XmlDocument();
            MyDoc.LoadXml(headerText);
            XmlElement myElement = MyDoc.DocumentElement;

            System.ServiceModel.Channels.MessageHeader myHeader = MessageHeader.CreateHeader("Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", myElement, false);
            request.Headers.Add(myHeader);

            return Convert.DBNull;
        }
    }
}