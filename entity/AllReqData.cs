namespace aus_backend_core.entity
{
    public class AllReqData
    {

        public string fileCode{get;set;}
        public CardData[]leftCardDataList{get;set;}
        public CardData[]rightCardDataList{get;set;}

        public CardData[] getAllCards(){
            CardData[]resultData=new CardData[leftCardDataList.Length+rightCardDataList.Length];
            leftCardDataList.CopyTo(resultData, 0);//这种方法适用于所有数组
            rightCardDataList.CopyTo(resultData, leftCardDataList.Length);

            return resultData;
        }
    }
    public  class CardData{
        public RowData[]rowList{get;set;}
        public string radioVList{get;set;}

    }
    public  class RowData{
        public string inputValue{get;set;}
        public string rowValue{get;set;}

        public int rowType{get;set;}

        public RowItemData[] rowItemList{get;set;}

    }
    public class RowItemData{
        public string rowItemTextValue{get;set;}
        public string rowItemValue{get;set;}

    }
}