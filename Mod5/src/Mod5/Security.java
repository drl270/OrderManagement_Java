package Mod5;


public class Security
{
    private String symbol;
    private SecurityType securityType;
    
    public enum SecurityType 
    {
        OPTIONS,
        FUTURES_ON_OPTIONS,
        STOCK,
        FUTURES,
        BONDS,
        ETF
    }

    public Security(String symbol, SecurityType securityType) 
    {
        this.symbol = symbol;
        this.securityType = securityType;
    }

    public String getSymbol()
    {
        return symbol;
    }

    public void setSymbol(String symbol)
    {
        this.symbol = symbol;
    }

    public SecurityType getSecurityType()
    {
        return securityType;
    }

    public void setSecurityType(SecurityType securityType)
    {
        this.securityType = securityType;
    }
}