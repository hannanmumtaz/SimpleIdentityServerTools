makecert -n "CN=Lokit CA,O=AdvICT,OU=Dev" -cy authority -a sha1 -sv "SidCa.pvk" -r "SidCa.cer"
makecert -n "CN=localhost" -ic "SidCa.cer" -iv "SidCa.pvk" -a sha1 -sky exchange -pe -sv "certificate_prk.pvk" "certificate_puk.cer"
pvk2pfx -pvk "certificate_prk.pvk" -spc "certificate_puk.cer" -pfx "certificate_prk.pfx"