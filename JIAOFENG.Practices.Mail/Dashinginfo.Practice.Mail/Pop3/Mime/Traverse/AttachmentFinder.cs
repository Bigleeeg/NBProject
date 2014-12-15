﻿using System;
using System.Collections.Generic;

namespace JIAOFENG.Practices.Mail.Pop3.Mime.Traverse
{
	/// <summary>
	/// Finds all <see cref="MessagePart"/>s which are considered to be attachments
	/// </summary>
	internal class AttachmentFinder : MultipleMessagePartFinder
	{
		protected override List<MessagePart> CaseLeaf(MessagePart messagePart)
		{
			if (messagePart == null)
				throw new ArgumentNullException("messagePart");

			// Maximum space needed is one
			List<MessagePart> leafAnswer = new List<MessagePart>(1);

			if (messagePart.IsAttachment)
				leafAnswer.Add(messagePart);

			return leafAnswer;
		}
	}
}