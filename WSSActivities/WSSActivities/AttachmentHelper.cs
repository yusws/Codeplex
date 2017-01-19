using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Microsoft.SharePoint;
using System.Net.Mail;
using Microsoft.SharePoint.Workflow;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint.WorkflowActions;
using Microsoft.SharePoint.Utilities;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.SharePoint.Administration;
using System.Diagnostics;

namespace MyLocalBroadband.Activities.WSS.Utilities
{
	class AttachmentHelper
    {

        /// <summary>
        /// gets all of the attachments for a given list item
        /// </summary>
        /// <param name="listItems"></param>
        /// <returns></returns>
        public static AttachmentInfo[] GetListItemAttachments(SPListItem listItem)
        {
            List<AttachmentInfo> myAttachments = new List<AttachmentInfo>();

            for (int i = 0; i < listItem.Attachments.Count; i++)
            {
                //attachments are actualy SPFiles stored in SPFolder that is part of our list
                SPFile myAttachmentFile = listItem.Web.GetFile(listItem.Attachments.UrlPrefix + listItem.Attachments[i]);

                AttachmentInfo myAI = new AttachmentInfo(myAttachmentFile.OpenBinaryStream(), myAttachmentFile.Name);

                myAttachments.Add(myAI);
            }

            return myAttachments.ToArray();
        }

        public static void DeleteAllAttachments(SPListItem listItem)
        {
            foreach(string attachment in listItem.Attachments)
            {
                listItem.Attachments.Recycle(attachment);
            }
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }
	}
    internal struct AttachmentInfo
    {
        public Stream Stream;
        public string FileName;

        public AttachmentInfo(Stream _stream, string _fileName)
        {
            this.Stream = _stream;
            this.FileName = _fileName;


        }
    }

}
