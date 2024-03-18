public struct Question2
{
    public string text;
    public string[] answers;
    public int correctAnswer;

    public Question2(string text, string[] answers, int correctAnswer)
    {
        this.text = text;
        this.answers = answers;
        this.correctAnswer = correctAnswer;
    }
}

public struct Quiz2
{
    public string title;
    public Question2[] Question2s;

    public Quiz2(string title, Question2[] Question2s) {
        this.title = title;
        this.Question2s = Question2s;
    }
}

public class MicroscopeQuiz2 {

    public static Quiz2[] Quiz2 = {
        new Quiz2(
                    "COMPONENTES E USO",
                    new Question2 [] {
                                    new Question2(
                                                    "Acerca dos componentes e uso de microscópio óptico, marque a opção correta:", 
                                                    new string [] {
                                                        "A. As objetivas e oculares são consideradas partes óticas.",
                                                        "B. O macrometro permite fazer o ajuste fino do foco.",
                                                        "C. A lâmina deve ser posicionada na platina com a lamínula para baixo.",
                                                        "D. A charriot deve ser usado para troca de objetiva. "
                                                    }, 
                                                    0
                                                ),
                                    new Question2(
                                                    "Acerca do uso de microscópio óptico, marque a opção errada:", 
                                                    new string [] {
                                                        "A. Para utilizar a lente objetiva de imersão, um óleo deve ser colocado entre a lâmina e a objetiva. ",
                                                        "B. É necessário girar o macrometro ou o micrometro, conforme o grau de ajuste necessário, para focalizar a imagem da amostra.",
                                                        "C. O revólver permite ajustar o foco da imagem observada pelas oculares.",
                                                        "D. Charriot é peça associada à platina cuja função é movimentar a lâmina no plano horizontal."
                                                    }, 
                                                    2
                                                ),
                                    new Question2(
                                                    "Com relação aos princípios básicos da microscopia óptica e o uso de microscópio óptico, marque a opção correta:", 
                                                    new string [] {
                                                        "A. A platina serve para orientar e espalhar a luz sobre o espécime em análise.",
                                                        "B. Para centralizar a amostra no campo de visão, deve-se utilizar o macrometro.",
                                                        "C.	As oculares aumentam a imagem fornecida pela lente objetiva, e é considerada um componente mecânico.",
                                                        "D.	A ampliação total da imagem observada em microscópio óptico consiste no produto da ampliação da objetiva pela ampliação da ocular."
                                                    }, 
                                                    3
                                                )
                                }
                        ),
        new Quiz2("LIMPEZA",
                new Question2 [] {
                                new Question2(
                                                "De acordo com os procedimentos de limpeza de microscópio óptico, assinale a alternativa incorreta:", 
                                                new string [] {
                                                    "A.	Certificar-se que o microscópio esteja preferencialmente desligado durante a limpeza.",
                                                    "B.	Utilizar gaze, algodão ou papel toalha para limpeza dos componentes ópticos.",
                                                    "C.	Esperar que os solventes evaporem antes do uso do microscópio.",
                                                    "D.	Depois do uso o microscópio deve ser protegido com capas contra poeira e sujeira."
                                                }, 
                                                1
                                            ),
                                new Question2(
                                                "Para a limpeza de microscópio óptico não se deve usar certos solventes capazes de penetrar na área de fixação das lentes dissolvendo a cola, e também produzir uma película sobre elas. Uma dessas substâncias descritas é:", 
                                                new string [] {
                                                    "A.	Álcool isopropílico",
                                                    "B.	Álcool absoluto",
                                                    "C.	Xilol",
                                                    "D.	Água destilada"
                                                }, 
                                                2
                                            ),
                                new Question2(
                                                "Antes e após o uso do microscópio, a limpeza das lentes, das objetivas e das oculares, deve ser feita preferencialmente com:", 
                                                new string [] {
                                                    "A.	Xilol ou água destilada",
                                                    "B.	Álcool isopropílico ou álcool absoluto",
                                                    "C.	Álcool 70% ou álcool absoluto",
                                                    "D.	Acetona ou Álcool isopropílico"
                                                }, 
                                                1
                                            ),
                            }),
        };
}