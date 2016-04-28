
#define ERR	     	-1
#define ACKOUTTIME	-2
#define ACKERR		-3
#define COMMOUTTIME	-4
#define STXERR      -5
#define OK	    	0

#define ERRCARD	2
#define ERRRDCARD	3
#define ERRWRCARD	4
#define ERRCARDSEC	5
#define ERRCARDKEY	6
#define ERRCARDLOCKED 7
#define ERRMSG 8
#define ERRRFCARD	9
#define ERRFORMAT	10
#define ERROVERFLOW	11

#define NOCARD	1
#define UNKNOWCARD	12
#define ERRCARDPOSITION	14

#define PAC_ADDRESS	1021

#define ENQ  0x05//请求连接通信线路(询问).
#define ACK  0x06//确认(握手).
#define NAK  0x15//通信忙.
#define EOT  0x04//通信结束(传送结束).
#define CAN  0x18//解除通信(取消).
#define STX  0x02//数据包起始符(起始字符).
#define ETX  0x03//数据包结束符(终结符).
#define US   0x1F//数据分隔符.

int APIENTRY GetSysVerion(HANDLE ComHandle, char *strVerion);
HANDLE APIENTRY CommOpen(char *Port);
HANDLE APIENTRY CommOpenWithBaut(char *Port, unsigned int _data);
HANDLE APIENTRY CommOpenWithBautForVB(HANDLE ComHandle, BYTE _Bauddata);
int APIENTRY CommClose(HANDLE ComHandle);
int APIENTRY CommSetting(HANDLE ComHandle,char *ComSeting);
////////////////////////////////////////////////////////////////////////////////////////

int APIENTRY WBM5000_ResetVerion(HANDLE ComHandle,  BYTE _Verion[]);
int APIENTRY WBM5000_ResetMove(HANDLE ComHandle, BYTE Reset_Type);
int APIENTRY WBM5000_CardIN(HANDLE ComHandle, BYTE CardIn_Type1, BYTE CardIn_Type2, BYTE *ERR_Code);
int APIENTRY WBM5000_CardMove(HANDLE ComHandle, BYTE Move_Type, BYTE *ERR_Code);
int APIENTRY WBM5000_CardStop(HANDLE ComHandle, BYTE Stop_Type, BYTE *ERR_Code);
int APIENTRY WBM5000_CardState(HANDLE ComHandle, BYTE *State1, BYTE *State2, BYTE *State3);
int APIENTRY WBM5000_SensorState(HANDLE ComHandle, BYTE SenState[], BYTE *ERR_Code);

int APIENTRY WBM5000_ICPowerON(HANDLE ComHandle, BYTE *ERR_Code);
int APIENTRY WBM5000_ICPowerOFF(HANDLE ComHandle, BYTE *ERR_Code);
int APIENTRY WBM5000_CPUCardReset(HANDLE ComHandle, BYTE *RLEN, BYTE ResetData[], BYTE *ERR_Code);
int APIENTRY WBM5000_CPUCardAPDU(HANDLE ComHandle, BYTE APDUSendData[], BYTE *RLEN, BYTE APDURecData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SIMCardReset(HANDLE ComHandle, BYTE *RLEN, BYTE CardNumber, BYTE ResetData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SIMCardAPDU(HANDLE ComHandle,  BYTE CardNumber, BYTE APDUSendData[], BYTE *RLEN, BYTE APDURecData[], BYTE *ERR_Code);
int APIENTRY WBM5000_AT24XXRead(HANDLE ComHandle, BYTE CardType, BYTE RLEN, BYTE ADDRH, BYTE ADDRL, BYTE ReadData[], BYTE *ERR_Code);
int APIENTRY WBM5000_AT24XXWrite(HANDLE ComHandle, BYTE CardType, BYTE WLEN, BYTE ADDRH, BYTE ADDRL, BYTE WriteData[], BYTE *ERR_Code);
int APIENTRY WBM5000_AT24XXCheckWrite(HANDLE ComHandle, BYTE CardType, BYTE WLEN, BYTE ADDRH, BYTE ADDRL, BYTE WriteData[], BYTE ReadData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4442Reset(HANDLE ComHandle, BYTE RecData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4442Read(HANDLE ComHandle, BYTE RLEN, BYTE ADDR, BYTE ReadData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4442Write(HANDLE ComHandle, BYTE WLEN, BYTE ADDR, BYTE WriteData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4442CheckPW(HANDLE ComHandle, BYTE PassWord[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4442ChangePW(HANDLE ComHandle, BYTE PassWord[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4442WriteProtect(HANDLE ComHandle, BYTE WLEN, BYTE ADDR, BYTE WriteData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4442ReadProtect(HANDLE ComHandle, BYTE ReadData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4442ReadPSC(HANDLE ComHandle, BYTE ReadData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4428Reset(HANDLE ComHandle, BYTE RecData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4428Read(HANDLE ComHandle, BYTE RLEN, BYTE ADDRH, BYTE ADDRL, BYTE ReadData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4428Write(HANDLE ComHandle, BYTE WLEN, BYTE ADDRH, BYTE ADDRL, BYTE WriteData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4428CheckPW(HANDLE ComHandle, BYTE PassWord[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4428ChangePW(HANDLE ComHandle, BYTE OldPassWord[], BYTE NewPassWord[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4428WriteProtect(HANDLE ComHandle, BYTE WLEN, BYTE ADDRH, BYTE ADDRL, BYTE WriteData[], BYTE *ERR_Code);
int APIENTRY WBM5000_SLE4428ReadProtect(HANDLE ComHandle, BYTE RLEN, BYTE ADDRH, BYTE ADDRL, BYTE ReadData[], BYTE *ERR_Code);

int APIENTRY WBM5000_RFScanCard(HANDLE ComHandle, BYTE *ERR_Code);
int APIENTRY WBM5000_RFGetSN(HANDLE ComHandle, BYTE SNData[], BYTE *ERR_Code);
int APIENTRY WBM5000_RFCheckPW(HANDLE ComHandle, BYTE Mode, BYTE Sector, BYTE PassWord[], BYTE *ERR_Code);
int APIENTRY WBM5000_RFChangePW(HANDLE ComHandle, BYTE Sector, BYTE PassWord[], BYTE *ERR_Code);
int APIENTRY WBM5000_RFReadData(HANDLE ComHandle, BYTE Sector, BYTE Block, BYTE ReadData[], BYTE *ERR_Code);
int APIENTRY WBM5000_RFWriteData(HANDLE ComHandle, BYTE Sector, BYTE Block, BYTE Block_Data[], BYTE *ERR_Code);
int APIENTRY WBM5000_RFAddVal(HANDLE ComHandle, BYTE Sector, BYTE Block, BYTE Val_Data[], BYTE *ERR_Code);
int APIENTRY WBM5000_RFDecVal(HANDLE ComHandle, BYTE Sector, BYTE Block, BYTE Val_Data[], BYTE *ERR_Code);
int APIENTRY WBM5000_RFInitVal(HANDLE ComHandle, BYTE Sector, BYTE Block, BYTE Block_Data[], BYTE *ERR_Code);
int APIENTRY WBM5000_RFReadVal(HANDLE ComHandle, BYTE Sector, BYTE Block, BYTE ReadData[], BYTE *ERR_Code);

int APIENTRY WBM5000_MagCardReadData(HANDLE ComHandle, BYTE Mode, BYTE Track_Type, BYTE *RLEN, BYTE TrackData[]);


















