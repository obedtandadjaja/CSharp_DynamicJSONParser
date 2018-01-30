using System;
using System.Collections.Generic;
using System.Collections;

public class Utility {

	public static JSONParserObject ParseJSONString(string json, char? endingChar) {
        json = json.Trim();
        if(json.Length > 0) {
            
            if(json[0] == ',') json = json.Substring(1).Trim();
            
            // when next object is an object
            if(json[0] == '{') {
                Dictionary<string, object> currentObject = new Dictionary<string, object>();
                json = json.Substring(1).Trim();
                while(json[0] != '}') {
                    if(json[0] == ',') json = json.Substring(1).Trim();
                    if(json[0] == '\"') {
                        json = json.Substring(1);
                        int indexOfNextQuotation = json.IndexOf('\"');
                        if(indexOfNextQuotation == -1) throw new Exception("JSON format error. Parsing stopped at: " + json);
                        string fieldName = json.Substring(0, indexOfNextQuotation);
                        json = json.Substring(indexOfNextQuotation+1).Trim();
                        
                        if(json[0] == ':') {
                            json = json.Substring(1).Trim();
                            JSONParserObject parsedObject = ParseJSONString(json, null);
                            currentObject[fieldName] = parsedObject.currentObject;
                            json = parsedObject.json.Trim();
                        } else {
                            throw new Exception("JSON format error. Parsing stopped at: " + json);
                        }
                        
                    } else {
                        throw new Exception("JSON format error. Parsing stopped at: " + json);
                    }
                }
                json = json.Substring(1);
                return new JSONParserObject(json, currentObject);
             
            // when next object is an array
            } else if(json[0] == '[') {
                ArrayList currentObject = new ArrayList();
                json = json.Substring(1).Trim();
                while(json[0] != ']') {
                    JSONParserObject parsedObject = ParseJSONString(json, null);
                    currentObject.Add(parsedObject.currentObject);
                    json = parsedObject.json.Trim();
                    
                    if(json[0] == ',') json = json.Substring(1).Trim();
                }
                json = json.Substring(1);
                return new JSONParserObject(json, currentObject);

            // when next object is a string
            } else if(json[0] == '\"') {
                json = json.Substring(1);
                int indexOfNextQuotation = json.IndexOf('\"');
                if(indexOfNextQuotation == -1) throw new Exception("JSON format error. Parsing stopped at: " + json);
                string currentObject = json.Substring(1, indexOfNextQuotation);
                json = json.Substring(indexOfNextQuotation+1).Trim();
                return new JSONParserObject(json, currentObject);

            // when next object is null
            } else if (json.Substring(0, 4).Equals("null")) {
                json = json.Substring(4);
                return new JSONParserObject(json, null);

            // when next object is a number
            } else if (char.IsDigit(json[0])) {
                int indexOfComma = json.IndexOf(',');
                int num = int.Parse(json.Substring(0, indexOfComma + 1));
                json = json.Substring(indexOfComma + 1);
                return new JSONParserObject(json, num);

            } else {
                throw new Exception("JSON format error. Parsing stopped at: " + json);
            }
        } else {
            return null;
        }
    }
    
    public class JSONParserObject {
        public string json;
        public object currentObject;
        public JSONParserObject(string json, object currentObject) {
            this.json = json;
            this.currentObject = currentObject;
        }
    }

}