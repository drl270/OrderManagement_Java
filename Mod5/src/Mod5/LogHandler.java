package Mod5;

import java.io.FileWriter;
import java.io.IOException;
import java.io.Writer;
import java.util.logging.Handler;
import java.util.logging.LogRecord;
import java.util.logging.Logger;

public class LogHandler extends Handler
{
    private final LogFormatter formatter;
    private final Writer writer;

    public LogHandler(LogFormatter formatter, String fileName) throws IOException
    {
        this.formatter = formatter;
        this.writer = new FileWriter(fileName, true); 
    }

    @Override
    public void publish(LogRecord record) 
    {
        try 
        {
            writer.write(formatter.format(record));
        } 
        catch (IOException e) 
        {
            System.err.println("Error writing to file: " + e.getMessage());
        }
    }

    @Override
    public void flush()
    {
        try 
        {
            writer.flush();
        } 
        catch (IOException e) 
        {
            System.err.println("Error flushing file: " + e.getMessage());
        }
    }

    @Override
    public void close() throws SecurityException
    {
        try 
        {
            writer.close();
        } 
        catch (IOException e) 
        {
            System.err.println("Error closing file: " + e.getMessage());
        }
    }
}