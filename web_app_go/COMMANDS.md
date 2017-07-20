docker build -t go-guestbook .
docker run -d -e "SQLSERVER=ip.address" -e "SQLPORT=10433" -e "SQLID=sa" -e "SQLPWD=yourpassword" -e "SQLDB=sql_guestbook" --name web -p 80:8001 go-guestbook