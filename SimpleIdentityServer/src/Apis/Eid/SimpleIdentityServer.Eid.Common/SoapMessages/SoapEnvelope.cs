namespace SimpleIdentityServer.Eid.Common.SoapMessages
{
    public class SoapEnvelope
    {
        public SoapEnvelope(SoapHeader header, SoapBody body)
        {
            Header = header;
            Body = body;
        }

        public SoapHeader Header { get; private set; }
        public SoapBody Body { get; private set; }
    }
}
