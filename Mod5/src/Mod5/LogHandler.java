package Mod5;

import java.util.logging.Handler;
import java.util.logging.LogRecord;
import java.util.logging.Logger;

public class LogHandler extends Handler 
{
    private final LogFormatter formatter;

    public LogHandler(LogFormatter formatter)
    {
        this.formatter = formatter;
    }

    @Override
    public void publish(LogRecord record) 
    {
        System.out.println(formatter.format(record));
    }

    @Override
    public void flush() 
    {
        // Add Code
    }

    @Override
    public void close() throws SecurityException
    {
        // Add Code
    }
}