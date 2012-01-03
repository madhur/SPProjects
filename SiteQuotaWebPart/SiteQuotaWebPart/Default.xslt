<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>
  
    <xsl:template match="SPWeb">
      <xsl:processing-instruction name="xml-stylesheet">
      type="text/css"
      </xsl:processing-instruction>
      
          <p/>
            <br/>
              <center>
                <table style='width:200px;height:20px' class='QG' cellspacing='0' cellpadding='0'>
                  <tr>
                    <td class='ms-storMeUsed' width='{(Storage div StorageMaximumLeveL)*200}'> </td>
                    <td class='ms-storMeFree' width='{200-(Storage div StorageMaximumLeveL)*200}'></td>
                  </tr>
                </table>
          <p/>
          <p/>
            <span class='ms-pageinformationheader'>Quota Used: <xsl:value-of select="round((Storage div StorageMaximumLeveL)*100)"/> %
          </span>
          <p/>
          </center>

    </xsl:template>
</xsl:stylesheet>
