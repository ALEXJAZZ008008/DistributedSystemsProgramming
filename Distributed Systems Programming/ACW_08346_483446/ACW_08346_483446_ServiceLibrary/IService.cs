using System.ServiceModel;

namespace ACW_08346_483446_ServiceLibrary
{
    //This is the interface for the server
    [ServiceContract]
    public interface IService
    {
        //This is the interface item for the HELLO method of the server
        [OperationContract]
        string HELLO(int id);

        //This is the interface item for the SORT method of the server
        [OperationContract]
        string SORT(string[] value);

        //This is the interface item for the PUBKEY method of the server
        [OperationContract]
        byte[][] PUBKEY();

        //This is the interface item for the ENC method of the server
        [OperationContract]
        void ENC(byte[] message);

        //This is the interface item for the SHA1 method of the server
        [OperationContract]
        string SHA1(string message);

        //This is the interface item for the SHA256 method of the server
        [OperationContract]
        string SHA256(string message);

        //This is the interface item for the SIGN method of the server
        [OperationContract]
        byte[] SIGN(string message);
    }
}