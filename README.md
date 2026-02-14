MyCsharpConsoleApp
專案簡介
本專案為早期在驊宏時期開發的 C# 控制台應用程式，主要用途是自動化處理圖像資料並將其封裝至 XML 格式文件中。此外，專案中也包含了一系列通用的工具類別，用於資料庫操作、檔案管理及郵件發送。

主要功能
圖像 Base64 轉換與封裝：讀取指定目錄下的圖片（支援 .jpg, .png），將其轉換為 Base64 編碼，並依照 p.xml 樣板插入到主 XML 文件中。

資料庫操作類別 (DBFun)：封裝了對 Microsoft SQL Server 的常用操作，包含 DataTable 讀取、標量值取得與非查詢指令執行，並支援交易處理。

檔案處理工具 (FileFun)：提供二進位物件序列化與目錄檢查等常用功能。

郵件發送測試 (MailFun)：實作了透過 SMTP 發送帶有附件的電子郵件功能。

檔案說明
Program.cs: 程式主進入點，負責 XML 合併與圖片處理邏輯。

Doc.config: 存放程式執行所需的配置資訊（如輸出檔名、圖片路徑等）。

doc.xml / p.xml: XML 樣板檔案。

list.txt: 待處理的圖片清單。

README (English Version)
MyCsharpConsoleApp
Project Overview
This project is a legacy C# console application developed during my time at Azion (驊宏). Its primary purpose is to automate the process of encoding image data into Base64 format and embedding it into structured XML documents. It also serves as a collection of utility classes for database, file, and email operations.

Key Features
Image-to-Base64 XML Packaging: Automatically reads images (supporting .jpg and .png) listed in list.txt, converts them to Base64 strings, and populates them into an XML structure based on predefined templates (doc.xml and p.xml).

Database Utility (DBFun): A robust wrapper for SQL Server operations, supporting DataTable filling, ExecuteNonQuery, ExecuteScalar, and transaction management with IDisposable implementation.

File Utility (FileFun): Includes methods for binary object serialization/deserialization and directory management.

Mail Utility (MailFun): A utility for sending automated emails with attachments via SMTP (configured for Gmail).

Core Files
Program.cs: The main entry point containing the logic for file processing and string template replacement.

Doc.config: Configuration file defining output names, source paths, and image dimensions.

doc.xml / p.xml: Template files for the XML structure.

list.txt: A plain text list identifying the images to be processed.
