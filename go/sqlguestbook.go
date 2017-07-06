package main

import (
	"database/sql"
	"fmt"
	"net/http"
	"os"

	"log"

	_ "github.com/denisenkom/go-mssqldb"
)

var (
	date    string
	name    string
	phone   string
	message string
	score   string
)

func indexHandler(w http.ResponseWriter, r *http.Request) {

	// gather values
	var hostname = getHostname()
	var appversion = "1.0"
	// need code to handle empty envvars
	var sqlserver = os.Getenv("SQLSERVER")
	var sqlid = os.Getenv("SQL_ID")
	var sqlpwd = os.Getenv("SQL_PWD")
	var connString = "server=" + sqlserver + ";user id=" + sqlid + ";password=" + sqlpwd + ";database=sql_guestbook;connection timeout=30"

	fmt.Fprintf(w, "<!DOCTYPE html><html><head><style>table {font-family: arial, sans-serif;border-collapse: collapse;width: 100%;}td, th {border: 1px solid #dddddd;text-align: left;padding: 8px;}tr:nth-child(even) {background-color: #dddddd;}</style></head><body>")
	fmt.Fprintf(w, "<h1>Golang Guestbook (v%s)</h1><p>Hostname: %s</p><table><tr><th>Date</th><th>Name</th><th>Phone</th><th>Sentiment</th><th>Message</th></tr>", appversion, hostname)

	// query DB and loop through rows
	conn, err := sql.Open("mssql", connString)
	if err != nil {
		log.Fatal("Open connection failed:", err.Error())
	}
	defer conn.Close()

	rows, err := conn.Query("SELECT * FROM guestlog")
	if err != nil {
		log.Fatal("Cannot query: ", err.Error())
		return
	}
	defer rows.Close()

	for rows.Next() {
		err := rows.Scan(&date, &name, &phone, &message, &score)
		if err != nil {
			log.Fatal(err)
		}
		// log.Println(date, name, phone, message, score)
		fmt.Fprintf(w, "<tr><td>"+date+"</td><td>"+name+"</td><td>"+phone+"</td><td>"+score+"</td><td>"+message+"</td></tr>")
	}
	fmt.Fprintf(w, "</table>")
}

func main() {
	http.HandleFunc("/", indexHandler)
	http.ListenAndServe(":8001", nil)
}

func getHostname() string {
	var result string
	localhostname, err := os.Hostname()

	if err != nil {
		result = "ERROR: Cannot find server hostname"
	} else {
		result = localhostname
	}
	return result
}
