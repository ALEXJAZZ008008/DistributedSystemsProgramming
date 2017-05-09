using System.ServiceModel;

namespace ACW_08346_483446_ServiceLibrary
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string HELLO(int id);

        [OperationContract]
        string SORT(int number, string[] value);

        [OperationContract]
        string PUBKEY();

        [OperationContract]
        void ENC(string message);

        [OperationContract]
        string SHA1(string message);

        [OperationContract]
        string SHA256(string message);

        [OperationContract]
        string SIGN(string message);
    }
}