# CSharp_DynamicJSONParser
C# JSON dynamic parser - need not to predefine JSON structure on parse

I needed a JSON parser to parse JSON API responses that is still under development and so the responses change from time to time. Other libraries would break when this happens because the predefined structure of the expected JSON no longer match the actual response. 

This is project is my attempt to make a JSON parser that only requires the user to define its structure post-parse (i.e. on usage).

<b>Idea</b>: return everything as objects on parsing; assign types only on usage.

## Use case
```c#
// create API request
HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(API_REQUEST_URL);
http.Method = "GET";

// get API response
HttpWebResponse response = (HttpWebResponse)http.GetResponse();
using(var streamReader = new StreamReader(response.GetResponseStream()))
{
    // returns JSON string
    string result = streamReader.ReadToEnd();
    JSONParserObject obj = ParseJSONString(result, null); // <-- parsing
    
    // define JSON structure on usage
    Dictionary<string, object> response = obj.currentObject as Dictionary<string, object>;
    string name = (string)response["name"];
    string username = (string)response["username"];
    string bio = (string)response["bio"];
    ArrayList achievements = (ArrayList)response["achievements"];
}  
```

## Compare to other libraries
#### JSON.NET
```c#
class Product {
  public string Name { get; set; }
  public DateTime ExpiryDate { get; set; }
  public double Price { get; set; }
  public string[] Sizes { get; set; }
}

Product product = new Product();
product.Name = "Apple";
product.ExpiryDate = new DateTime(2008, 12, 28);
product.Price = 3.99M;
product.Sizes = new string[] { "Small", "Medium", "Large" };

// convert to JSON string
string output = JsonConvert.SerializeObject(product);

// JSON string to Product
Product deserializedProduct = JsonConvert.DeserializeObject<Product>(output);
```
