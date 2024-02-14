namespace MyPersonalWebAPI.Util
{
    public class Util:IUtil
    {
        public object TextMessage(string message, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "text",
                text = new
                {
                    body = message
                }
            };
        }

        public object ImageMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "image",
                image = new
                {
                    link = url
                }
            };
        }

        public object AudioMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "audio",
                audio = new
                {
                    link = url
                }
            };
        }

        public object VideoMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "video",
                video = new
                {
                    link = url
                }
            };
        }


        public object DocumentMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "document",
                document = new
                {
                    link = url
                }
            };
        }

        public object TemplateMessage(string number, string name_template, object[] Parameters)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "template",
                template = new
                {
                    name = "recordatorios",
                    language = new
                    {
                        code = "es_MX"
                    },
                    components = new[]
                    {
                        new
                        {
                            type="body",
                            parameters = Parameters
                        }
                    }

                }
            };
        }

        public object LocationMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "location",
                location = new
                {
                    latitude = "19.718959842622382",
                    longitude = "-101.2333991204397",
                    name = "Estadio Morelos",
                    address = "58147 Morelia, Mich."
                }
            };
        }

        // example button
        public object ButtonsMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "button",
                    body = new
                    {
                        text = "Selecciona una opcion"
                    },
                    action = new
                    {
                        buttons = new List<object>
                        {
                            new
                            {
                                type= "reply",
                                reply= new
                                {
                                    id = "01",
                                    title="Comprar",
                                }
                            },
                            new
                            {
                                type= "reply",
                                reply= new
                                {
                                    id = "02",
                                    title="Vender",
                                }
                            },
                        }
                    }
                }
            };
        }
    }
}
