using System;
using System.Linq;
using System.Text;
using System.Web;
using LINQtoCSV;

namespace LoadNeuseCSV
{
    internal class ProductCSV
    {
        [CsvColumn(Name = "brandName", FieldIndex = 1)]
        public string BrandName { get; set; }

        [CsvColumn(Name = "modelName", FieldIndex = 2)]
        public string ModelName { get; set; }

        [CsvColumn(Name = "modelYear", FieldIndex = 3)]
        public string ModelYear { get; set; }

        [CsvColumn(Name = "modelSKU", FieldIndex = 4)]
        public string ModelSku { get; set; }

        [CsvColumn(Name = "modelDescription", FieldIndex = 5)]
        public string ModelDescription { get; set; }

        [CsvColumn(Name = "modelImage", FieldIndex = 6)]
        public string NodelImage { get; set; }

        [CsvColumn(Name = "imageCaption", FieldIndex = 7)]
        public string ImageCaption { get; set; }

        [CsvColumn(Name = "gender", FieldIndex = 8)]
        public string Gender { get; set; }

        [CsvColumn(Name = "categoryID", FieldIndex = 9)]
        public int CategoryID { get; set; }

        [CsvColumn(Name = "sku", FieldIndex = 10)]
        public string Sku { get; set; }

        [CsvColumn(Name = "mpn", FieldIndex = 11)]
        public string mpn { get; set; }

        [CsvColumn(Name = "gtin1", FieldIndex = 12)]
        public string Gtin1 { get; set; }

        [CsvColumn(Name = "gtin2", FieldIndex = 13)]
        public string Gtin2 { get; set; }

        [CsvColumn(Name = "msrp", FieldIndex = 14)]
        public double Msrp { get; set; }

        [CsvColumn(Name = "dealerCost", FieldIndex = 15)]
        public double DealerCost { get; set; }

        [CsvColumn(Name = "specialCost", FieldIndex = 16)]
        public string SpecialCost { get; set; }

        [CsvColumn(Name = "lowMsrp", FieldIndex = 17)]
        public double LowMsrp { get; set; }

        [CsvColumn(Name = "length", FieldIndex = 18)]
        public string Length { get; set; }

        [CsvColumn(Name = "width", FieldIndex = 19)]
        public string Width { get; set; }

        [CsvColumn(Name = "height", FieldIndex = 20)]
        public string Height { get; set; }

        [CsvColumn(Name = "weight", FieldIndex = 21)]
        public double Weight { get; set; }

        [CsvColumn(Name = "image", FieldIndex = 22)]
        public string Image { get; set; }

        [CsvColumn(Name = "unit", FieldIndex = 23)]
        public string Unit { get; set; }

        [CsvColumn(Name = "hazmatCode", FieldIndex = 24)]
        public string HazmatCode { get; set; }

        [CsvColumn(Name = "taxable", FieldIndex = 25)]
        public string Taxable { get; set; }

        [CsvColumn(Name = "shippable", FieldIndex = 26)]
        public string Shippable { get; set; }

        [CsvColumn(Name = "shipGround", FieldIndex = 27)]
        public string ShipGround { get; set; }

        [CsvColumn(Name = "shipAir", FieldIndex = 28)]
        public string ShipAir { get; set; }

        [CsvColumn(Name = "ormd", FieldIndex = 29)]
        public string Ormd { get; set; }

        [CsvColumn(Name = "FFLrequired", FieldIndex = 30)]
        public string FfLrequired { get; set; }

        [CsvColumn(Name = "NFArequired", FieldIndex = 31)]
        public string NfArequired { get; set; }

        [CsvColumn(Name = "variHash", FieldIndex = 32)]
        public string VariHash { get; set; }

        [CsvColumn(Name = "name", FieldIndex = 33)]
        public string Name { get; set; }

        [CsvColumn(Name = "id", FieldIndex = 34)]
        public string Id { get; set; }

        [CsvColumn(Name = "text", FieldIndex = 35)]
        public string Text { get; set; }
    }

    internal class SectionCsv
    {
        [CsvColumn(Name = "id", FieldIndex = 1)]
        public int id { get; set; }

        [CsvColumn(Name = "parent", FieldIndex = 2)]
        public int parent { get; set; }

        [CsvColumn(Name = "level", FieldIndex = 3)]
        public int level { get; set; }

        [CsvColumn(Name = "type", FieldIndex = 4)]
        public string type { get; set; }

        [CsvColumn(Name = "code", FieldIndex = 5)]
        public string code { get; set; }

        [CsvColumn(Name = "description", FieldIndex = 6)]
        public string description { get; set; }

        [CsvColumn(Name = "sort", FieldIndex = 7)]
        public string sort { get; set; }
    }

    internal class Program
    {
        public static int counter = 0;

        private static void Main(string[] args)
        {
            // query data from sections

            var dd = new GHDataDataContext();
            IQueryable<GHData> byDescription1 =
                from a in dd.GetTable<GHData>()
                where a.SKU != null
                select a;

// Data is now available via variable products.

            foreach (GHData item in byDescription1)
            {
                //Data maping object to our database
                var text = new ProductDataContext();
                var myProduct = new product();

                var dc = new ProductDataContext();
                IQueryable<product> q =
                    from a in dc.GetTable<product>()
                    where a.pID.Equals(item.UPC)
                    select a;
                if (q.Any())
                {
                    foreach (product x in q)
                    {
                        buildData(x, item);
                        dc.SubmitChanges();
                        saveImage(item);
                        ++counter;
                        Console.WriteLine(counter);
                    }
                }
                else
                {
                    ++counter;
                    Console.WriteLine(counter);
                    buildData(myProduct, item);
                    text.products.InsertOnSubmit(myProduct);
                    text.SubmitChanges();
                    saveImage(item);
                }
            }
        }

        // executes the appropriate commands to implement the changes to the database

        private static int Asc(String ch)
        {
            //Return the character value of the given character
            ch = ch.ToUpper();
            byte[] bytes = Encoding.ASCII.GetBytes(ch);
            string numString = "";
            int j;
            foreach (byte b in bytes)
            {
                if (b >= 48 && b <= 57)
                {
                    numString = numString + (Convert.ToChar(b));
                }
                else
                {
                    j = b - 64;
                    numString = numString + (Convert.ToString(j));
                }
            }

            return Convert.ToInt32(numString);
        }

        private static void buildData(product myProduct, GHData item)
        {
            myProduct.pID = item.UPC;
            myProduct.pName = "Guy Harvey - " + HttpUtility.UrlDecode(item.StyleDescription) + " - " + item.Color +
                              " - " + item.Size;
            myProduct.pName2 = myProduct.pName;
            myProduct.pSection = 26;

            myProduct.pDescription = myProduct.pName;

            myProduct.pLongdescription = myProduct.pName;
            myProduct.pSKU = item.SKU;

            myProduct.pPrice = Convert.ToDouble(item.MSRP);
            myProduct.pListPrice = 0;

            myProduct.pWeight = 2;
            myProduct.pTax = 0;
            myProduct.pWholesalePrice = 0;
            myProduct.pShipping = 0;
            myProduct.pShipping2 = 0;
            myProduct.pStaticPage = false;
            myProduct.pStockByOpts = false;
            myProduct.pRecommend = false;
            myProduct.pGiftWrap = false;
            myProduct.pBackOrder = false;
            myProduct.pOrder = 0;

            myProduct.pDisplay = 1;
            myProduct.pSell = 1;

            myProduct.pManufacturer = 1;
            myProduct.pDropship = 0;

            myProduct.pSearchParams = "Guy Harvey - " + HttpUtility.UrlDecode(item.StyleDescription);
        }

        private static void saveImage(GHData item)
        {
            var text2 = new productImagesDataContext();
            var dc = new productImagesDataContext();

            var myProductImage = new productimage();

            IQueryable<productimage> q =
                from a in dc.GetTable<productimage>()
                where a.imageProduct.Equals(item.UPC) && (a.imageType == 0)
                select a;
            if (q.Any())
            {
                foreach (productimage x in q)
                {
                    buildImage(x, item, 0);
                    dc.SubmitChanges();
                }
            }
            else
            {
                buildImage(myProductImage, item, 0);
                text2.productimages.InsertOnSubmit(myProductImage);
                text2.SubmitChanges();
            }

            var text3 = new productImagesDataContext();
            var dc2 = new productImagesDataContext();
            myProductImage = new productimage();

            IQueryable<productimage> z =
                from a in dc2.GetTable<productimage>()
                where a.imageProduct.Equals(item.UPC) && (a.imageType == 1)
                select a;
            if (z.Any())
            {
                foreach (productimage x in z)
                {
                    buildImage(x, item, 1);
                    dc2.SubmitChanges();
                }
            }
            else
            {
                buildImage(myProductImage, item, 1);
                text3.productimages.InsertOnSubmit(myProductImage);
                text3.SubmitChanges();
            }
        }

        private static void buildImage(productimage myProductImage, GHData item, short imgTyp)
        {
            myProductImage.imageNumber = 0;
            myProductImage.imageType = imgTyp;
            string size = "s";
            switch (imgTyp)
            {
                case 0:
                    size = "s";
                    break;
                case 1:
                    size = "l";
                    break;
                default:
                    size = "s";
                    break;
            }

            myProductImage.imageSrc = "guyharveyimages/" + size + item.SKU.Trim() + item.Color.Trim() + ".gif";
            myProductImage.imageProduct = item.UPC;
        }
    }
}